using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using EAITMApp.Application.Persistence;
using EAITMApp.Infrastructure.Persistence;
using EAITMApp.Infrastructure.Settings;
using EAITMApp.Infrastructure.Persistence.Providers;
using EAITMApp.Application.Persistence.Validation;
using EAITMApp.Infrastructure.Persistence.Validation;

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
            services.AddScoped<IWriteDbContext, WriteDbContext>();

            // ReadDbContext
            ConfigureDbContextForProvider<ReadDbContext>(services, settings, s => s.ReadDatabaseSettings, noTracking: true);
            services.AddScoped<IReadDbContext, ReadDbContext>();

            // Settings
            services.AddSingleton<IDatabaseSettingsValidator, DatabaseSettingsValidator>();
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
                    throw new InvalidOperationException($"Provider '{provider.ProviderType}' does not support EF Core.");

                var connectionString = provider.BuildConnectionString(dbSettings);
                efProvider.ConfigureDbContext(options, connectionString);

                if (noTracking) options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
        }
    }
}
