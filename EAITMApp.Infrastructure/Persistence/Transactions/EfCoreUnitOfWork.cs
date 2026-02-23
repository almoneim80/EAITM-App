using EAITMApp.Application.Persistence;
using EAITMApp.Application.Persistence.Transactions;
using Microsoft.EntityFrameworkCore;

namespace EAITMApp.Infrastructure.Persistence.Transactions
{
    public sealed class EfCoreUnitOfWork(WriteDbContext dbContext, IEnumerable<ITransactionHook> hooks) : IUnitOfWork
    {
        private readonly WriteDbContext _dbContext = dbContext;
        private readonly IEnumerable<ITransactionHook> _hooks = hooks;

        public async Task<T> ExecuteAsync<T>(Func<CancellationToken, Task<T>> operation, CancellationToken cancellationToken)
        {
            var strategy = _dbContext.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async (ct) =>
            {
                await using var transaction = await _dbContext.Database.BeginTransactionAsync(ct);
                var context = CreateContext();
                try
                {
                    var result = await operation(ct);

                    foreach (var hook in _hooks)
                        await hook.BeforeCommitAsync(context);

                    await _dbContext.SaveChangesAsync(ct);
                    await transaction.CommitAsync(ct);

                    foreach (var hook in _hooks)
                        await hook.AfterCommitAsync(context);

                    return result;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(ct);
                    foreach (var hook in _hooks)
                        await hook.OnRollbackAsync(context, ex);

                    throw;
                }
            }, cancellationToken);
        }

        public Task ExecuteAsync(Func<CancellationToken, Task> operation, CancellationToken cancellationToken)
        {
            return ExecuteAsync<object?>(async ct =>
            {
                await operation(ct);
                return null;
            }, cancellationToken);
        }

        private static ITransactionContext CreateContext() => new TransactionContext();
    }
}
