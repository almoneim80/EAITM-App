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

            // seeders
            services.AddSeeders();
        }

        /// <summary>
        /// Configures a <typeparamref name="TContext"/> with a dynamically resolved database provider and global settings.
        /// Handles connection string building, naming conventions, performance behaviors (NoTracking), and auditing interceptors.
        /// </summary>
        /// <typeparam name="TContext">The type of the DbContext to configure.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <param name="settings">The root data store settings containing all database configurations.</param>
        /// <param name="selectSettings">A delegate to select specific connection settings (e.g., Read vs. Write) from the root settings.</param>
        /// <param name="noTracking">If set to <c>true</c>, configures the context to ignore change tracking for better read performance.</param>
        /// <exception cref="InvalidOperationException">Thrown when the resolved provider does not implement <see cref="IEFCoreRelationalProvider"/>.</exception>
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

        /// <summary>
        /// Automatically registers all implementations of <see cref="IDataSeeder"/> from the assembly.
        /// </summary>
        /// <param name="services">The service collection to register seeders into.</param>
        /// <returns>The <see cref="IServiceCollection"/> for method chaining.</returns>
        private static IServiceCollection AddSeeders(this IServiceCollection services)
        {
            // Get all calsses that immplement IDataSeeder from current Assembly
            var seeders = typeof(DatabaseRegistration).Assembly.GetTypes()
                .Where(t => typeof(IDataSeeder).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            foreach(var seeder in seeders)
            {
                services.AddScoped(typeof(IDataSeeder), seeder);
            }

            return services;
        }
    }
}
