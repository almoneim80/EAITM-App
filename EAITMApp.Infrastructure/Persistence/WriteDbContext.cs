using EAITMApp.Application.Persistence;
using EAITMApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EAITMApp.Infrastructure.Persistence
{
    /// <summary>
    /// EF Core DbContext optimized for write operations.
    /// Supports CQRS and replication scenarios.
    /// </summary>
    public class WriteDbContext : DbContext, IWriteDbContext
    {
        public WriteDbContext(DbContextOptions<WriteDbContext> options)
            : base(options) { }

        /// <inheritdoc/>
        public DbSet<TEntity> Set<TEntity>() where TEntity : class => base.Set<TEntity>();

        /// <inheritdoc/>
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => base.SaveChangesAsync(cancellationToken);

        /// <inheritdoc/>
        public Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
            => base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

        /// <inheritdoc/>
        public new DatabaseFacade Database => base.Database;

        /// <inheritdoc/>
        public new ChangeTracker ChangeTracker => base.ChangeTracker;

        /// <inheritdoc/>
        public DbSet<TodoTask> TodoTasks { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply entity configurations
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(WriteDbContext).Assembly);
        }
    }
}
