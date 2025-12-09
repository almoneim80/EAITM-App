using EAITMApp.Infrastructure.Repositories.Settings.Providers;
using EAITMApp.Infrastructure.Repositories.Settings;
using Microsoft.Extensions.DependencyInjection;
using EAITMApp.Infrastructure.Factories;

namespace EAITMApp.Infrastructure.Data
{
    public static class DatabaseConfiguration
    {
        /// <summary>
        /// Configures both write (primary) and read (replica) databases using DI and the provider factory.
        /// Enforces type consistency and supports CQRS/Replication.
        /// </summary>
        /// <param name="services">DI service collection.</param>
        /// <param name="dataStores">Strongly-typed data stores settings.</param>
        public static void ConfigureDatabases(
                    IServiceCollection services,
                    DataStoresSettings dataStores,
                    IDatabaseProviderFactory factory)   // تمرير المصنع من DI
        {
            if (dataStores == null) throw new ArgumentNullException(nameof(dataStores));

            if (!string.Equals(
                    dataStores.WriteDatabaseSettings.ProviderType,
                    dataStores.ReadDatabaseSettings.ProviderType,
                    StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Database types must match.");

            // Configure Write DbContext
            factory.GetProvider(dataStores.WriteDatabaseSettings.ProviderType)
                   .RegisterWrite(services, dataStores.WriteDatabaseSettings);

            // Configure Read DbContext
            factory.GetProvider(dataStores.ReadDatabaseSettings.ProviderType)
                   .RegisterRead(services, dataStores.ReadDatabaseSettings);
        }
    }
}
}
