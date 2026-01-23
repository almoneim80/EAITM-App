using EAITMApp.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace EAITMApp.Infrastructure.Persistence.Providers
{
    /// <summary>
    /// PostgreSQL database provider implementation.
    /// </summary>
    public sealed class PostgresDatabaseProvider : IRelationalDatabaseProvider, IEFCoreRelationalProvider
    {
        public string ProviderType => "Postgres";
        public string BuildConnectionString(IDatabaseConnectionSettings s)
        {
            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = s.Host,
                Port = s.Port,
                Database = s.Database,
                Username = s.Username,
                Password = s.Password,
                Pooling = s.Pooling
            };

            // SslMode
            if (!Enum.TryParse<SslMode>(s.SslMode, true, out var ssl))
            {
                throw new InvalidOperationException(
                    $"Invalid SslMode '{s.SslMode}' provided for database provider '{ProviderType}'. " +
                    $"Valid values are: {string.Join(", ", Enum.GetNames(typeof(SslMode)))}"
                );
            }
            builder.SslMode = ssl;

            // AdditionalSettings
            foreach (var kvp in s.AdditionalSettings)
                builder[kvp.Key] = kvp.Value;

            return builder.ConnectionString;
        }

        public void ConfigureDbContext(DbContextOptionsBuilder options, string connectionString) 
            => options.UseNpgsql(connectionString);
    }
}
