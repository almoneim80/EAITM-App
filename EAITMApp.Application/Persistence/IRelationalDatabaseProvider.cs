namespace EAITMApp.Application.Persistence
{
    /// <summary>
    /// Represents a database provider abstraction.
    /// </summary>
    public interface IRelationalDatabaseProvider
    {
        string ProviderType { get; }

        string BuildConnectionString(IDatabaseConnectionSettings settings);
    }
}
