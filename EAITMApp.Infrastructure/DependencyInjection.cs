using EAITMApp.Application.Interfaces;
using EAITMApp.Infrastructure.Configurations;
using EAITMApp.Infrastructure.Data;
using EAITMApp.Infrastructure.Repositories;
using EAITMApp.Infrastructure.Repositories.Settings;
using EAITMApp.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using EAITMApp.Infrastructure.Repositories.TaskRepo;
using EAITMApp.Infrastructure.Repositories.UserRepo;

namespace EAITMApp.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // =========================
            // Load configuration sections
            // =========================
            var storageSettingsSection = configuration.GetSection("StorageSettings");
            services.Configure<DataStoresSettings>(storageSettingsSection);
            var storageSettings = storageSettingsSection.Get<DataStoresSettings>() ?? new DataStoresSettings();

            services.Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings"));
            services.AddSingleton(sp => sp.GetRequiredService<IOptions<MongoDbSettings>>().Value);

            services.AddDbContext<PrimaryDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("PostgresConnection")));
            services.AddScoped<IAppDbContext, PrimaryDbContext>();

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
            // ITodoTaskRepository
            services.AddScoped<ITodoTaskRepository>(sp =>
            { var dbContext = sp.GetRequiredService<IAppDbContext>(); return new TodoTaskRepository(dbContext); });

            // IUserRepository
            services.AddScoped<IUserRepository>(sp =>
            { var dbContext = sp.GetRequiredService<IAppDbContext>(); return new UserRepository(dbContext); });

            return services;
        }
    }
}