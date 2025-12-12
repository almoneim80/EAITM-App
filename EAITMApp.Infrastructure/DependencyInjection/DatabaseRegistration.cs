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
            services.AddDbContext<WriteDbContext>(options => options.UseNpgsql(BuildConnectionString(settings.WriteDatabaseSettings)));
            services.AddScoped<IWriteDbContext, WriteDbContext>();

            // ReadDbContext
            services.AddDbContext<ReadDbContext>(options =>
            {
                options.UseNpgsql(BuildConnectionString(settings.ReadDatabaseSettings));
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
            services.AddScoped<IReadDbContext, ReadDbContext>();
        }

        /// <summary>
        /// Builds a connection string from the provided settings.
        /// </summary>
        private static string BuildConnectionString(IDatabaseConnectionSettings s)
        {
            if(string.IsNullOrWhiteSpace(s.Host) ||
               s.Port <= 0 ||
               string.IsNullOrWhiteSpace(s.Database) ||
               string.IsNullOrWhiteSpace(s.Username) ||
               string.IsNullOrWhiteSpace(s.Password)) 
            {
                throw new 
                    InvalidOperationException("One or more required database settings are missing " +
                    "or invalid (Host, Port, Database, Username, Password).");
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

            foreach (var kvp in s.AdditionalSettings)
            {
                //if (builder.ContainsKey(kvp.Key))
                //{
                //    continue;
                //}

                builder[kvp.Key] = kvp.Value;
            }

            return builder.ConnectionString;
        }
    }
}
