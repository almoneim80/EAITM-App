using EAITMApp.Application.Interfaces;
using EAITMApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
namespace EAITMApp.Infrastructure.Data
{
    public class PrimaryDbContext : DbContext, IAppDbContext
    {
        public PrimaryDbContext(DbContextOptions<PrimaryDbContext> options) : base(options) {}

        public DbSet<TodoTask> TodoTasks { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        public new DbSet<T> Set<T>() where T : class => base.Set<T>();

        public new Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => base.SaveChangesAsync(cancellationToken);

        public new Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
            => base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

        public DatabaseFacade Database => base.Database;

        public ChangeTracker ChangeTracker => base.ChangeTracker;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TodoTask>(builder =>
            {
                builder.HasKey(t => t.Id);
                builder.Property(t => t.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");
            });

            modelBuilder.Entity<User>(builder =>
            {
                builder.HasKey(t => t.Id);
                builder.Property(t => t.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");
            });
        }
    }
}
