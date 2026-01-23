using EAITMApp.Application.Persistence;

namespace EAITMApp.Infrastructure.Persistence.Providers
{
    /// <summary>
    /// Resolves database providers based on provider type.
    /// </summary>
    public sealed class DatabaseProviderFactory(IEnumerable<IRelationalDatabaseProvider> providers) : IDatabaseProviderFactory
    {
        private readonly IEnumerable<IRelationalDatabaseProvider> _providers = providers;

        public IRelationalDatabaseProvider GetProvider(string providerType)
        {
            var provider = _providers.FirstOrDefault(provider => string.Equals(provider.ProviderType, providerType,
                StringComparison.OrdinalIgnoreCase));


            if (provider == null)
                throw new InvalidOperationException($"Unsupported database provider '{providerType}'.");

            return provider;
        }
    }
}
