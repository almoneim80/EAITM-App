using EAITMApp.Domain.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace EAITMApp.Infrastructure.Persistence.Extensions
{
    public static class ModelBuilderExtensions
    {
        /// <summary>
        /// Applies all global configurations like Soft Delete filters and Custom Conventions.
        /// </summary>
        public static void ApplyGlobalFilters(this ModelBuilder modelBuilder)
        {
            /// <summary>
            /// Iterates through all registered entities in the DbContext model and applies 
            /// global configurations (like Soft Delete filters) to those implementing specific interfaces.
            /// </summary>
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (!typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType)) continue;

                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var property = Expression.Property(parameter, nameof(ISoftDelete.IsDeleted));
                var condition = Expression.Equal(property, Expression.Constant(false));
                var lambda = Expression.Lambda(condition, parameter);

                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
            }
        }
    }
}
