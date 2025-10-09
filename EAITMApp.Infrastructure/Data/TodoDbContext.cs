using EAITMApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace EAITMApp.Infrastructure.Data
{
    public class TodoDbContext : DbContext
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
        {
        }

        public DbSet<TodoTask> TodoTasks { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TodoTask>()
                .Property(t => t.Id)
                .HasColumnType ("gen_random_uuid()");
        }
    }
}
