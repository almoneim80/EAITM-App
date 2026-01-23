namespace EAITMApp.Application.Persistence
{
    /// <summary>
    /// Represents a transactional unit of work.
    /// </summary>
    public interface IUnitOfWork
    {
        Task ExecuteAsync(Func<CancellationToken, Task> operation, CancellationToken cancellationToken = default);
        Task<T> ExecuteAsync<T>(Func<CancellationToken, Task<T>> operation, CancellationToken cancellationToken = default);
    }
}
