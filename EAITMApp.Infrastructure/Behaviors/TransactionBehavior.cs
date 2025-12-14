using EAITMApp.Application.Interfaces;
using EAITMApp.Infrastructure.Persistence;
using MediatR;

namespace EAITMApp.Infrastructure.Behaviors
{
    public class TransactionBehavior<TRequest, TResponse> 
        : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull, ITransactionalCommand
    {
        // access to write database from related DbContext
        private readonly WriteDbContext _context;
        public TransactionBehavior(WriteDbContext context){ _context = context; }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // start Transaction
            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var response = await next();
                await transaction.CommitAsync(cancellationToken);
                return response;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}
