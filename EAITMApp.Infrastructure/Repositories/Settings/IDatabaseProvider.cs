using EAITMApp.Infrastructure.Repositories.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace EAITMApp.Application.Interfaces
{
    /// <summary>
    /// Represents a database provider responsible for registering EF Core DbContexts
    /// for both write and read operations. Supports dependency injection.
    /// </summary>
    public interface IDatabaseProvider
    {
        /// <summary>
        /// Registers the DbContext for write operations (Primary database).
        /// </summary>
        /// <param name="services">The DI service collection.</param>
        /// <param name="settings">Connection settings for the write database.</param>
        void RegisterWrite(IServiceCollection services, IDatabaseConnectionSettings settings);

        /// <summary>
        /// Registers the DbContext for read operations (Read replica database).
        /// </summary>
        /// <param name="services">The DI service collection.</param>
        /// <param name="settings">Connection settings for the read database.</param>
        void RegisterRead(IServiceCollection services, IDatabaseConnectionSettings settings);
    }
}
