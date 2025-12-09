using EAITMApp.Application.Interfaces;
using EAITMApp.Infrastructure.Data;
using EAITMApp.Infrastructure.Repositories.Settings;
using EAITMApp.Infrastructure.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EAITMApp.Infrastructure.Repositories.TaskRepo;
using EAITMApp.Infrastructure.Repositories.UserRepo;
using EAITMApp.Infrastructure.Factories;
using EAITMApp.Infrastructure.Repositories.Settings.Providers;

namespace EAITMApp.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // =========================
            // Load configuration sections
            // =========================

            var dataStoresSection = configuration.GetSection("DataStores");
            services.Configure<DataStoresSettings>(dataStoresSection);
            var dataStores = dataStoresSection.Get<DataStoresSettings>() ?? new DataStoresSettings();

            // =========================
            // Configure databases (Write & Read) using DatabaseConfiguration
            // =========================
            var factory = new DatabaseProviderFactory();
            factory.RegisterProvider("postgres", new PostgresProvider());
            services.AddSingleton<IDatabaseProviderFactory>(factory);

            // تكوين قواعد البيانات باستخدام المصنع المسجل
            DatabaseConfiguration.ConfigureDatabases(services, dataStores, factory);

            // =========================
            // Security settings
            // =========================
            services.Configure<Argon2Settings>(configuration.GetSection("Argon2Settings"));

            // =========================
            // Automatic registration via Scrutor
            // =========================
            services.Scan(scan => scan
                .FromAssembliesOf(typeof(DependencyInjection))
                .AddClasses(classes => classes.InNamespaces(
                    "EAITMApp.Infrastructure.Memory",
                    "EAITMApp.Infrastructure.Security",
                    "EAITMApp.Infrastructure.Factories"))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            // =========================
            // Repository registration logic
            // =========================
            services.AddScoped<ITodoTaskRepository>(sp =>
            {
                var dbContext = sp.GetRequiredService<IWriteDbContext>();
                return new TodoTaskRepository(dbContext);
            });

            services.AddScoped<IUserRepository>(sp =>
            {
                var dbContext = sp.GetRequiredService<IWriteDbContext>();
                return new UserRepository(dbContext);
            });

            return services;
        }
    }
}
