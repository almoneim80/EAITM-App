using EAITMApp.Domain.Common;
using Microsoft.EntityFrameworkCore;
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
                if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
                {
                    var method = typeof(ModelBuilderExtensions)
                        .GetMethod(nameof(SetSoftDeleteFilter),BindingFlags.NonPublic | BindingFlags.Static)
                        ?.MakeGenericMethod(entityType.ClrType);

                    method?.Invoke(null, new object[] { modelBuilder });
                }
            }
        }

        private static void SetSoftDeleteFilter<TEntity>(ModelBuilder modelBuilder) where TEntity : class, ISoftDelete
        {
            modelBuilder.Entity<TEntity>().HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
