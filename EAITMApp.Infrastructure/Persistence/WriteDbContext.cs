using EAITMApp.Application.Persistence;
using Microsoft.EntityFrameworkCore;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply entity configurations
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(WriteDbContext).Assembly);
        }
    }
}
