namespace EAITMApp.Application.Persistence.Repositories
{
    /// <summary>
    /// Write-only repository interface.
    /// Responsible for commands (CUD) only.
    /// No querying logic allowed here by design.
    /// </summary>
    /// <typeparam name="TEntity">Aggregate Root / Entity</typeparam>
    /// <typeparam name="TKey">Primary Key type</typeparam>
    public interface IWriteRepository<TEntity, TKey> where TEntity : class
    {
        /// <summary>
        /// Create single object.
        /// </summary>
        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Create multiple objects.
        /// </summary>
        Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);


        /// <summary>
        /// Update single object.
        /// </summary>
        void Update(TEntity entity);

        /// <summary>
        /// Update multiple objects.
        /// </summary>
        void UpdateRange(IEnumerable<TEntity> entities);


        /// <summary>
        /// Delete single object.
        /// </summary>
        void Remove(TEntity entity);

        /// <summary>
        /// Delete multiple objects.
        /// </summary>
        void RemoveRange(IEnumerable<TEntity> entities);
    }
}
