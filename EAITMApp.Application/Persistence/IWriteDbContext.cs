using EAITMApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

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

        /// <summary>
        /// Saves all changes made in this context with the option to accept all changes automatically.
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">Whether to accept changes automatically.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The number of state entries written to the database.</returns>
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);

        /// <summary>
        /// Provides access to database-specific operations, such as transactions or raw SQL execution.
        /// </summary>
        DatabaseFacade Database { get; }

        /// <summary>
        /// Provides access to change tracking information for entities in this context.
        /// </summary>
        ChangeTracker ChangeTracker { get; }

        /// <summary>
        /// Optional: Define common DbSets here for convenience, but avoid hardcoding entity types in large-scale projects.
        /// </summary>
        DbSet<TodoTask> TodoTasks { get; set; }
        DbSet<User> Users { get; set; }
    }
}
