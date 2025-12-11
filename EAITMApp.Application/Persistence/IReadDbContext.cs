using EAITMApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EAITMApp.Application.Persistence
{
    /// <summary>
    /// Represents a read-only DbContext for EF Core, optimized for CQRS/Replication scenarios.
    /// This interface focuses only on read operations and supports IQueryable for efficient querying.
    /// </summary>
    public interface IReadDbContext
    {
        /// <summary>
        /// Provides access to a DbSet for a specific entity type.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <returns>The DbSet for the entity type.</returns>
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        /// <summary>
        /// Finds an entity by its primary key asynchronously.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="keyValues">The primary key values.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The entity if found; otherwise, null.</returns>
        Task<TEntity?> FindAsync<TEntity>(object[] keyValues, CancellationToken cancellationToken = default)
            where TEntity : class;

        /// <summary>
        /// Provides access to common DbSets for convenience.
        /// These are optional and can be extended as needed.
        /// </summary>
        DbSet<TodoTask> TodoTasks { get; }
        DbSet<User> Users { get; }
    }
}
