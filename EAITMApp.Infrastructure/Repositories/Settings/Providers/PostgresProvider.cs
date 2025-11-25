using EAITMApp.Application.Interfaces;
using EAITMApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EAITMApp.Infrastructure.Repositories.Settings.Providers
{
    /// <summary>
    /// EF Core provider for PostgreSQL databases.
    /// Responsible for registering DbContexts for both write (primary) and read (replica) operations.
    /// </summary>
    public class PostgresProvider : IDatabaseProvider
    {
        /// <inheritdoc />
        public void RegisterWrite(IServiceCollection services, IDatabaseConnectionSettings settings)
        {
            var connStr = GetConnectionString(settings);

            services.AddDbContext<WriteDbContext>(options =>
                options.UseNpgsql(connStr));

            services.AddScoped<IWriteDbContext, WriteDbContext>();
        }

        /// <inheritdoc />
        public void RegisterRead(IServiceCollection services, IDatabaseConnectionSettings settings)
        {
            var connStr = GetConnectionString(settings);

            services.AddDbContext<ReadDbContext>(options =>
                options.UseNpgsql(connStr));

            services.AddScoped<IReadDbContext, ReadDbContext>();
        }

        /// <summary>
        /// Retrieves the PostgreSQL connection string from settings.
        /// </summary>
        /// <param name="settings">Database connection settings.</param>
        /// <returns>Connection string.</returns>
        /// <exception cref="InvalidOperationException">Thrown if connection string is missing or invalid.</exception>
        private static string GetConnectionString(IDatabaseConnectionSettings settings)
        {
            if (settings.Options.TryGetValue("ConnectionString", out var connObj) && connObj is string connStr)
                return connStr;

            throw new InvalidOperationException("Postgres connection string not found in settings.");
        }
    }
}
