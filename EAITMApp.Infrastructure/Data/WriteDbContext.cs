using EAITMApp.Application.Interfaces;
using EAITMApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace EAITMApp.Infrastructure.Data
{
    /// <summary>
    /// EF Core DbContext optimized for write operations.
    /// Supports CQRS and replication scenarios.
    /// </summary>
    public class WriteDbContext : DbContext, IWriteDbContext
    {
        public WriteDbContext(DbContextOptions<WriteDbContext> options)
            : base(options) { }

        /// <summary>
        /// Provides access to a DbSet for a given entity type.
        /// Generic implementation allows adding new entities without modifying the DbContext.
        /// </summary>
        /// <typeparam name="TEntity">Entity type.</typeparam>
        /// <returns>DbSet for the entity.</returns>
        public DbSet<TEntity> Set<TEntity>() where TEntity : class => base.Set<TEntity>();

        /// <summary>
        /// Saves all changes asynchronously.
        /// </summary>
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => base.SaveChangesAsync(cancellationToken);

        /// <summary>
        /// Saves all changes asynchronously with option to accept all changes on success.
        /// </summary>
        public Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
            => base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

        /// <summary>
        /// Provides access to database-specific operations.
        /// </summary>
        public new DatabaseFacade Database => base.Database;

        /// <summary>
        /// Provides access to change tracking information.
        /// </summary>
        public new ChangeTracker ChangeTracker => base.ChangeTracker;

        /// <summary>
        /// Common DbSets can be defined here for convenience, optional for CQRS/Generic usage.
        /// </summary>
        public DbSet<TodoTask> TodoTasks { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure TodoTask entity
            modelBuilder.Entity<TodoTask>(builder =>
            {
                builder.HasKey(t => t.Id);
                builder.Property(t => t.Id)
                       .HasColumnType("uuid")
                       .HasDefaultValueSql("gen_random_uuid()");
            });

            // Configure User entity
            modelBuilder.Entity<User>(builder =>
            {
                builder.HasKey(u => u.Id);
                builder.Property(u => u.Id)
                       .HasColumnType("uuid")
                       .HasDefaultValueSql("gen_random_uuid()");
            });

            // Future entities can be configured here without modifying the DbContext interface
        }
    }
}
