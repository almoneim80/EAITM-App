using EAITMApp.Application.Interfaces;

namespace EAITMApp.Infrastructure.Factories
{
    /// <summary>
    /// Factory for creating database providers in a flexible and testable way.
    /// Supports DI, lazy initialization, and extensibility.
    /// </summary>
    public interface IDatabaseProviderFactory
    {
        IDatabaseProvider GetProvider(string dbType);
    }
}
