using Microsoft.Extensions.DependencyInjection;
using EAITMApp.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using EAITMApp.Application.Persistence;
using EAITMApp.Infrastructure.Persistence;
using EAITMApp.Infrastructure.Settings;

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

            // بناء نص الاتصال (يمكن استخراجه في دالة مساعدة)
            var writeConnStr = $"Host={settings.WriteDatabaseSettings.Host};Port={settings.WriteDatabaseSettings.Port};Database={settings.WriteDatabaseSettings.Database};Username={settings.WriteDatabaseSettings.Username};Password={settings.WriteDatabaseSettings.Password}";
            var readConnStr = $"Host={settings.ReadDatabaseSettings.Host};Port={settings.ReadDatabaseSettings.Port};Database={settings.ReadDatabaseSettings.Database};Username={settings.ReadDatabaseSettings.Username};Password={settings.ReadDatabaseSettings.Password}";

            // WriteDbContext
            services.AddDbContext<WriteDbContext>(options => options.UseNpgsql(writeConnStr));
            services.AddScoped<IWriteDbContext, WriteDbContext>();

            // ReadDbContext
            services.AddDbContext<ReadDbContext>(options => options.UseNpgsql(readConnStr));
            services.AddScoped<IReadDbContext, ReadDbContext>();
        }
    }
}
