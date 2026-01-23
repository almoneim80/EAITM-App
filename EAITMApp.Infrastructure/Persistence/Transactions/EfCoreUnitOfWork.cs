using EAITMApp.Application.Persistence;
using EAITMApp.Application.Persistence.Transactions;

namespace EAITMApp.Infrastructure.Persistence.Transactions
{
    public sealed class EfCoreUnitOfWork(WriteDbContext dbContext, IEnumerable<ITransactionHook> hooks) : IUnitOfWork
    {
        private readonly IWriteDbContext _dbContext = dbContext;
        private readonly IEnumerable<ITransactionHook> _hooks = hooks;
    }
}
