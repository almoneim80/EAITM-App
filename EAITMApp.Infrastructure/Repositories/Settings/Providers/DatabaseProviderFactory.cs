using EAITMApp.Application.Interfaces;
using EAITMApp.Infrastructure.Factories;

namespace EAITMApp.Infrastructure.Repositories.Settings.Providers
{
    public class DatabaseProviderFactory : IDatabaseProviderFactory
    {
        private readonly IDictionary<string, IDatabaseProvider> _providers;

        /// <summary>
        /// Initializes the factory with a dictionary of providers.
        /// </summary>
        /// <param name="providers">A dictionary of registered database providers.</param>
        public DatabaseProviderFactory(IDictionary<string, IDatabaseProvider>? providers = null)
        {
            // Use a case-insensitive dictionary
            _providers = new Dictionary<string, IDatabaseProvider>(StringComparer.OrdinalIgnoreCase);

            if (providers != null)
            {
                foreach (var kv in providers)
                    _providers[kv.Key] = kv.Value;
            }
        }

        /// <summary>
        /// Registers a new database provider.
        /// </summary>
        /// <param name="dbType">Database type name (e.g., "postgres").</param>
        /// <param name="provider">Provider instance.</param>
        public void RegisterProvider(string dbType, IDatabaseProvider provider)
        {
            if (string.IsNullOrWhiteSpace(dbType))
                throw new ArgumentException("Database type must be provided.", nameof(dbType));
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));

            _providers[dbType] = provider;
        }

        /// <summary>
        /// Retrieves a database provider by type.
        /// </summary>
        /// <param name="dbType">Database type name.</param>
        /// <returns>IDatabaseProvider instance.</returns>
        public IDatabaseProvider GetProvider(string dbType)
        {
            if (string.IsNullOrWhiteSpace(dbType))
                throw new ArgumentException("Database type must be provided.", nameof(dbType));

            if (_providers.TryGetValue(dbType, out var provider))
                return provider;

            throw new NotSupportedException($"Database provider for '{dbType}' is not supported.");
        }
    }
}
