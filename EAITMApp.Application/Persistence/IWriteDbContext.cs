using Microsoft.EntityFrameworkCore;

namespace EAITMApp.Application.Persistence
{
    /// <summary>
    /// Represents a write-only DbContext for EF Core, optimized for CQRS and large-scale applications.
    /// This interface focuses only on write operations, transaction management, and change tracking.
    /// </summary>
    public interface IWriteDbContext
    {
        /// <summary>
        /// Provides access to a DbSet for a specific entity type.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <returns>The DbSet for the entity type.</returns>
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        /// <summary>
        /// Saves all changes made in this context to the database asynchronously.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The number of state entries written to the database.</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
