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
            services.Configure<StorageSettings>(storageSettingsSection);
            var storageSettings = storageSettingsSection.Get<StorageSettings>() ?? new StorageSettings();

            services.Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings"));
            services.AddSingleton(sp => sp.GetRequiredService<IOptions<MongoDbSettings>>().Value);
            services.AddDbContext<TodoDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("PostgresConnection")));

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
            if (storageSettings.EnableMultipleStores)
            {
                // ITodoTaskRepository
                services.AddSingleton<ITodoTaskRepository, InMemoryTodoTaskRepository>();
                services.AddSingleton<ITodoTaskRepository>(sp => { var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
                    return new MongoTodoTaskRepository(settings);});
                services.AddScoped<ITodoTaskRepository, PostgresTodoTaskRepository>();


                // IUserRepository
                services.AddSingleton<IUserRepository, InMemoryUserRepository>();
            }
            else
            {
                switch (storageSettings.DefaultStore)
                {
                    case "Mongo":
                        services.AddSingleton<ITodoTaskRepository>(sp =>
                        {
                            var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
                            return new MongoTodoTaskRepository(settings);
                        });
                        break;

                    case "Postgres":
                        services.AddScoped<ITodoTaskRepository>(sp =>
                        {
                            var dbContext = sp.GetRequiredService<TodoDbContext>();
                            return new PostgresTodoTaskRepository(dbContext);
                        });
                        break;

                    default: // InMemory
                        services.AddSingleton<ITodoTaskRepository, InMemoryTodoTaskRepository>();
                        services.AddSingleton<IUserRepository, InMemoryUserRepository>();
                        break;
                }
            }

            return services;
        }
    }
}
