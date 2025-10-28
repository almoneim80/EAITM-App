using EAITMApp.Application.Interfaces;
using EAITMApp.Infrastructure.Configurations;
using EAITMApp.Infrastructure.Data;
using EAITMApp.Infrastructure.Memory;
using EAITMApp.Infrastructure.Repositories.TaskRepo;
using EAITMApp.Infrastructure.Repositories.UserRepo;
using EAITMApp.Infrastructure.Repositories;
using EAITMApp.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using EAITMApp.Domain.Common;
using EAITMApp.Infrastructure.Factories;

namespace EAITMApp.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, RepositoryType repoType, IConfiguration configuration)
        {
            // Register Memory Management Service
            services.AddSingleton<ISecureMemoryService, SecureMemoryService>();

            // Register Argon2Encryption service & Argon2 settings
            services.Configure<Argon2Settings>(configuration.GetSection("Argon2Settings"));
            services.AddSingleton<IEncryptionService, Argon2EncryptionService>();
            

            // Register Repositories
            switch (repoType)
            {
                case RepositoryType.Mongo:
                    services.Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings"));
                    services.AddSingleton<ITodoTaskRepository>(sp =>
                    {
                        var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
                        return new MongoTodoTaskRepository(settings);
                    });

                    // services.AddSingleton<IUserRepository, InMemoryUserRepository>();
                    break;

                case RepositoryType.Postgres:
                    services.AddDbContext<TodoDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("PostgresConnection")));
                    services.AddScoped<ITodoTaskRepository, PostgresTodoTaskRepository>();
                    // services.AddScoped<IUserRepository, InMemoryUserRepository>();
                    break;

                default: // InMemory
                    services.AddSingleton<ITodoTaskRepository, InMemoryTodoTaskRepository>();
                    services.AddSingleton<IUserRepository, InMemoryUserRepository>();
                    break;
            }

            // Factory registration
            services.AddSingleton(typeof(IRepositoryFactory<>), typeof(RepositoryFactory<>));

            return services;
        }
    }
}
