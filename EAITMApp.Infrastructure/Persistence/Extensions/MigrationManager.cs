using EAITMApp.SharedKernel.Errors.Registries;
using EAITMApp.SharedKernel.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EAITMApp.Infrastructure.Persistence.Extensions
{
    /// <summary>
    /// Provides automated database schema management and synchronization.
    /// Implements environment-aware policies to ensure safe migration execution across Development and Production.
    /// </summary>
    public static class MigrationManager
    {
        public static async Task ApplyMigrationsAsync(this IHost app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var env = services.GetRequiredService<IHostEnvironment>();
            var logger = services.GetRequiredService<ILoggerFactory>().CreateLogger("Migration");

            // List of contexts we need to check (Write and Read)
            var contextTypes = new[] { typeof(WriteDbContext), typeof(ReadDbContext) };

            try
            {
                foreach (var type in contextTypes)
                {
                    if (services.GetRequiredService(type) is DbContext context)
                    {
                        logger.LogInformation("Checking migrations for {ContextName} in {Environment}...", type.Name, env.EnvironmentName);

                        if (env.IsDevelopment())
                        {
                            await context.Database.MigrateAsync();
                            logger.LogInformation("Migrations applied successfully for {ContextName}.", type.Name);
                        }
                        else
                        {
                            var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
                            if (pendingMigrations.Any())
                            {
                                logger.LogCritical("Production Safety Guard: There are pending migrations for {ContextName}! Application will stop.", type.Name);
                                throw new PersistenceConfigurationException(PersistenceErrors.PendingMigrations,
                                    new Dictionary<string, object>
                                    {
                                        { "Context", type.Name },
                                        { "PendingCount", pendingMigrations.Count() }
                                    });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while migrating the database.");
                throw new PersistenceConfigurationException(PersistenceErrors.MigrationFailed);
            }
        }
    }
}
