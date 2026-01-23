namespace EAITMApp.Application.Persistence.Transactions
{
    public interface ITransactionHook
    {
        Task BeforeCommitAsync(ITransactionContext context);
        Task AfterCommitAsync(ITransactionContext context);
        Task OnRollbackAsync(ITransactionContext context, Exception exception);
    }
}
