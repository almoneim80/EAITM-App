using EAITMApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EAITMApp.Application.Interfaces
{
    /// <summary>
    /// General interface for DbContext to support any EF Core database provider.
    /// </summary>
    public interface IAppDbContext
    {
        /// <summary>
        /// Provides access to a table/entity set.
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <returns>DbSet for the entity</returns>
        DbSet<T> Set<T>() where T : class;

        /// <summary>
        /// Saves all changes made in this context to the database asynchronously.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Number of state entries written to the database</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Saves all changes with option to accept changes on success.
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">Whether to accept changes automatically</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Number of state entries written to the database</returns>
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);

        /// <summary>
        /// Provides access to database-specific operations like transactions or raw SQL.
        /// </summary>
        DatabaseFacade Database { get; }

        /// <summary>
        /// Provides access to change tracking information for entities.
        /// </summary>
        ChangeTracker ChangeTracker { get; }

        /// <summary>
        /// Example DbSet for TodoTask entity.
        /// Add more DbSets as needed for your application.
        /// </summary>
        DbSet<Domain.Entities.TodoTask> TodoTasks { get; set; }
    }
}
