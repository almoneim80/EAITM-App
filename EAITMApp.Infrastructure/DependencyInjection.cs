using EAITMApp.Application.Interfaces;
using EAITMApp.Infrastructure.Configurations;
using EAITMApp.Infrastructure.Data;
using EAITMApp.Infrastructure.Memory;
using EAITMApp.Infrastructure.Repositories.TaskRepo;
using EAITMApp.Infrastructure.Repositories.UserRepo;
using EAITMApp.Infrastructure.Repositories;
using EAITMApp.Infrastructure.Repositories.Settings;
using EAITMApp.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using EAITMApp.Infrastructure.Factories;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;

namespace EAITMApp.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Load storage settings
            var storageSettingsSection = configuration.GetSection("StorageSettings");
            services.Configure<StorageSettings>(storageSettingsSection);
            var storageSettings = storageSettingsSection.Get<StorageSettings>() ?? new StorageSettings();

            // Load stores configs
            services.Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings"));
            services.AddDbContext<TodoDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("PostgresConnection")));


            // Common services
            services.AddSingleton<ISecureMemoryService, SecureMemoryService>();
            services.Configure<Argon2Settings>(configuration.GetSection("Argon2Settings"));
            services.AddSingleton<IEncryptionService, Argon2EncryptionService>();

            // Repository registration logic
            if (storageSettings.EnableMultipleStores)
            {
                // Register all repositories (multi-storage mode)

                services.AddSingleton<ITodoTaskRepository, InMemoryTodoTaskRepository>();
                services.AddSingleton<ITodoTaskRepository>(sp =>
                {
                    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
                    return new MongoTodoTaskRepository(settings);
                });

                // Register DbContext for Postgres
                
                services.AddScoped<ITodoTaskRepository, PostgresTodoTaskRepository>();
                services.AddSingleton<IUserRepository, InMemoryUserRepository>();
            }
            else
            {
                // Single storage mode
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

            // Factory registration
            services.AddSingleton(typeof(IRepositoryFactory<>), typeof(RepositoryFactory<>));

            return services;
        }
    }
}
