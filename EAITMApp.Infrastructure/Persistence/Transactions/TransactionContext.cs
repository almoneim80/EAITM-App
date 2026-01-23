using EAITMApp.Application.Persistence.Transactions;

namespace EAITMApp.Infrastructure.Persistence.Transactions
{
    public sealed class TransactionContext : ITransactionContext
    {
        public Guid TransactionId { get; } = Guid.NewGuid();
        public DateTimeOffset StartedAt { get; } = DateTimeOffset.UtcNow;
    }
}
