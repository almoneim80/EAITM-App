using EAITMApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EAITMApp.Infrastructure.Persistence.Configurations
{
    public class TodoTaskConfiguration : IEntityTypeConfiguration<TodoTask>
    {
        public void Configure(EntityTypeBuilder<TodoTask> builder)
        {
            // Configure TodoTask entity
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id)
                   .HasColumnType("uuid")
                   .HasDefaultValueSql("gen_random_uuid()");
        }
    }
}
