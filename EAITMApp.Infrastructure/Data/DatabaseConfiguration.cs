using EAITMApp.Infrastructure.Repositories.Settings.Providers;
using EAITMApp.Infrastructure.Repositories.Settings;
using Microsoft.Extensions.DependencyInjection;

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
        public static void ConfigureDatabases(IServiceCollection services, DataStoresSettings dataStores)
        {
            if (dataStores == null) throw new ArgumentNullException(nameof(dataStores));

            // Ensure write and read database types match
            if (!string.Equals(dataStores.WriteDatabaseSettings.ProviderType,
                               dataStores.ReadDatabaseSettings.ProviderType,
                               StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    $"Read database type ({dataStores.ReadDatabaseSettings.ProviderType}) " +
                    $"must match Write database type ({dataStores.WriteDatabaseSettings.ProviderType})."
                );
            }

            // Resolve the database provider factory from DI (or create a default one)
            var factory = new DatabaseProviderFactory();
            factory.RegisterProvider("postgres", new PostgresProvider());

            // Configure Write DbContext
            factory.GetProvider(dataStores.WriteDatabaseSettings.ProviderType)
                   .RegisterWrite(services, dataStores.WriteDatabaseSettings);

            // Configure Read DbContext
            factory.GetProvider(dataStores.ReadDatabaseSettings.ProviderType)
                   .RegisterRead(services, dataStores.ReadDatabaseSettings);
        }
    }
}
