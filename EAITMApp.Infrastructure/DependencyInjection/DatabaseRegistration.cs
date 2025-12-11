using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using EAITMApp.Application.Persistence;
using EAITMApp.Infrastructure.Persistence;
using EAITMApp.Infrastructure.Settings;
using Npgsql;

namespace EAITMApp.Infrastructure.DependencyInjection
{
    public static class DatabaseRegistration
    {
        /// <summary>
        /// Configures both write (primary) and read (replica) databases using DI and the provider factory.
        /// Enforces type consistency and supports CQRS/Replication.
        /// </summary>
        public static void ConfigureDatabases(IServiceCollection services, DataStoresSettings settings)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));

            if (!string.Equals(
                    settings.WriteDatabaseSettings.ProviderType,
                    settings.ReadDatabaseSettings.ProviderType,
                    StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Database types must match.");
            

            // WriteDbContext
            services.AddDbContext<WriteDbContext>(options => options.UseNpgsql(GetWriteConnStr(settings)));
            services.AddScoped<IWriteDbContext, WriteDbContext>();

            // ReadDbContext
            services.AddDbContext<ReadDbContext>(options => options.UseNpgsql(GetReadConnStr(settings)));
            services.AddScoped<IReadDbContext, ReadDbContext>();
        }

        private static string GetWriteConnStr(DataStoresSettings settings)
        {
            var s = settings.WriteDatabaseSettings;
            if(string.IsNullOrWhiteSpace(s.Host) ||
               s.Port <= 0 ||
               string.IsNullOrEmpty(s.Database) ||
               string.IsNullOrEmpty(s.Username) ||
               string.IsNullOrEmpty(s.Password) ||
               s.Pooling == false) 
            {
                throw new 
                    InvalidOperationException("One or more required database settings are missing " +
                    "or invalid (Host, Port, Database, Username, Password, Pooling).");
            }

            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = s.Host,
                Port = s.Port,
                Database = s.Database,
                Username = s.Username,
                Password = s.Password,
                SslMode = s.SslMode,
                Pooling = s.Pooling
            };

            return builder.ConnectionString;
        }

        private static string GetReadConnStr(DataStoresSettings settings)
        {
            if (string.IsNullOrEmpty(settings.ReadDatabaseSettings.Host) |
               settings.ReadDatabaseSettings.Port <= 0 |
               string.IsNullOrEmpty(settings.ReadDatabaseSettings.Database) |
               string.IsNullOrEmpty(settings.ReadDatabaseSettings.Username) |
               string.IsNullOrEmpty(settings.ReadDatabaseSettings.Password))
            {
                throw new InvalidOperationException("Database settings are invalid.");
            }

            return
                $"Host={settings.ReadDatabaseSettings.Host};" +
                $"Port={settings.ReadDatabaseSettings.Port};" +
                $"Database={settings.ReadDatabaseSettings.Database};" +
                $"Username={settings.ReadDatabaseSettings.Username};" +
                $"Password={settings.ReadDatabaseSettings.Password}" +
                $"SslMode={settings.ReadDatabaseSettings.AdditionalSettings["SslMode"]}" +
                $"Pooling={settings.ReadDatabaseSettings.AdditionalSettings["Pooling"]}";
        }
    }
}
