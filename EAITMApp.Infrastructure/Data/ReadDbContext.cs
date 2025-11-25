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

        /// <summary>
        /// Provides access to a DbSet for a given entity type.
        /// Generic implementation allows adding new entities without modifying the DbContext.
        /// </summary>
        /// <typeparam name="TEntity">Entity type.</typeparam>
        /// <returns>DbSet for the entity.</returns>
        public DbSet<TEntity> Set<TEntity>() where TEntity : class => base.Set<TEntity>();

        /// <summary>
        /// Common DbSets can be defined here for convenience.
        /// Optional: can be removed to rely fully on Generic DbSets.
        /// </summary>
        public DbSet<TodoTask> TodoTasks { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        /// <summary>
        /// Finds an entity by primary key asynchronously.
        /// </summary>
        /// <typeparam name="TEntity">Entity type.</typeparam>
        /// <param name="keyValues">Primary key values.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The entity if found; otherwise null.</returns>
        public Task<TEntity?> FindAsync<TEntity>(object[] keyValues, CancellationToken cancellationToken = default)
            where TEntity : class
        {
            return base.FindAsync<TEntity>(keyValues, cancellationToken).AsTask();
        }

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
