using EAITMApp.Application.Interfaces;
using EAITMApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EAITMApp.Infrastructure.Data
{
    /// <summary>
    /// EF Core DbContext optimized for read-only operations.
    /// Supports CQRS/Replication scenarios.
    /// </summary>
    public class ReadDbContext : DbContext, IReadDbContext
    {
        public ReadDbContext(DbContextOptions<ReadDbContext> options)
            : base(options) { }

        /// <inheritdoc/>
        public DbSet<TEntity> Set<TEntity>() where TEntity : class => base.Set<TEntity>();

        /// <inheritdoc/>
        public DbSet<TodoTask> TodoTasks { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        /// <inheritdoc/>
        public Task<TEntity?> FindAsync<TEntity>(object[] keyValues, CancellationToken cancellationToken = default)
            where TEntity : class
        {
            return base.FindAsync<TEntity>(keyValues, cancellationToken).AsTask();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply entity configurations
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ReadDbContext).Assembly);
        }
    }
}
