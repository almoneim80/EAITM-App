using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using EAITMApp.Application.Persistence;
using EAITMApp.Infrastructure.Persistence;
using EAITMApp.Infrastructure.Settings;
using EAITMApp.Infrastructure.Persistence.Providers;
using EAITMApp.Application.Persistence.Validation;
using EAITMApp.Infrastructure.Persistence.Validation;
using EAITMApp.Application.Persistence.Transactions;
using EAITMApp.Infrastructure.Persistence.Transactions;
using EAITMApp.Application.Persistence.Repositories;
using EAITMApp.Infrastructure.Persistence.Repositories;
using EAITMApp.Infrastructure.Persistence.Interceptors;
using EAITMApp.Infrastructure.Persistence.Seeding;

namespace EAITMApp.Infrastructure.DependencyInjection
{
    public static class DatabaseRegistration
    {
        /// <summary>
        /// Configures both write (primary) and read (replica) databases using DI and the provider factory.
        /// Enforces type consistency and supports CQRS/Replication.
        /// </summary>
        public static void ConfigureDatabases(IServiceCollection services, DataStoresSettings settings)
        {
            // register Providers
            services.AddSingleton<IRelationalDatabaseProvider, PostgresDatabaseProvider>();
            services.AddSingleton<IDatabaseProviderFactory, DatabaseProviderFactory>();

            // WriteDbContext
            ConfigureDbContextForProvider<WriteDbContext>(services, settings, s => s.WriteDatabaseSettings);

            // ReadDbContext
            ConfigureDbContextForProvider<ReadDbContext>(services, settings, s => s.ReadDatabaseSettings, noTracking: true);

            // Settings
            services.AddSingleton<IDatabaseSettingsValidator, DatabaseSettingsValidator>();

            // Unit of Work
            services.AddScoped<IUnitOfWork, EfCoreUnitOfWork>(sp =>
            {
                var context = sp.GetRequiredService<WriteDbContext>();
                var hooks = sp.GetServices<ITransactionHook>();
                return new EfCoreUnitOfWork(context, hooks);
            });

            // Repositories
            services.AddScoped(typeof(IReadRepository<>), typeof(EfReadRepository<>));
            services.AddScoped(typeof(IWriteRepository<,>), typeof(EfWriteRepository<,>));

            // Interceptors
            services.AddScoped<AuditingInterceptor>();

            // Seeders
            services.AddScoped<IDataSeeder, RoleSeeder>();
        }

        private static void ConfigureDbContextForProvider<TContext>(
            IServiceCollection services, 
            DataStoresSettings settings, 
            Func<DataStoresSettings, IDatabaseConnectionSettings> selectSettings,
            bool noTracking = false) where TContext : DbContext
        {
            services.AddDbContext<TContext>((serviceProvider, options) =>
            {
                var providerFactory = serviceProvider.GetRequiredService<IDatabaseProviderFactory>();
                var dbSettings = selectSettings(settings);
                var provider = providerFactory.GetProvider(dbSettings.ProviderType);

                if (provider is not IEFCoreRelationalProvider efProvider)
                {
                    // Validates that the provider supports EF Core Relational operations to prevent execution on incompatible types.
                    throw new InvalidOperationException($"Provider '{provider.ProviderType}' does not support EF Core.");
                }
                
                // Construct the connection string and configure DbContext fluently using the specific provider
                var connectionString = provider.BuildConnectionString(dbSettings);
                efProvider.ConfigureDbContext(options, connectionString);

                // Enable Snake Case naming convention
                options.UseSnakeCaseNamingConvention();

                if (noTracking)
                {
                    // Disable change tracking to improve performance for read-only scenarios.
                    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                }
                else
                {
                    // Register AuditingInterceptor to capture entity changes when tracking is enabled
                    var interceptor = serviceProvider.GetRequiredService<AuditingInterceptor>();
                    options.AddInterceptors(interceptor);
                }
            });
        }
    }
}
