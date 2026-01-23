namespace EAITMApp.Application.Persistence.Transactions
{
    public interface ITransactionContext
    {
        Guid TransactionId { get; }
        DateTimeOffset StartedAt { get; }
    }
}
