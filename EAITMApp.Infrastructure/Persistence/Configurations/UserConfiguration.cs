using EAITMApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EAITMApp.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Configure User entity
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id)
                   .HasColumnType("uuid")
                   .HasDefaultValueSql("gen_random_uuid()");
        }
    }
}
