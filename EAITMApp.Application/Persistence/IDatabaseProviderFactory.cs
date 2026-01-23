namespace EAITMApp.Application.Persistence
{
    /// <summary>
    /// Resolves the correct database provider based on provider type.
    /// </summary>
    public interface IDatabaseProviderFactory
    {
        IRelationalDatabaseProvider GetProvider(string providerType);
    }
}
