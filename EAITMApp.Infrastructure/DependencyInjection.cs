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
            services.AddScoped<IAppDbContext, TodoDbContext>();

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
                services.AddScoped<ITodoTaskRepository, InMemoryTodoTaskRepository>();
                services.AddScoped<ITodoTaskRepository>(sp => 
                { var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value; return new MongoTodoTaskRepository(settings); });
                services.AddScoped<ITodoTaskRepository>(sp => 
                { var dbContext = sp.GetRequiredService<IAppDbContext>(); return new PostgresTodoTaskRepository(dbContext); });



                // IUserRepository
                services.AddScoped<IUserRepository, InMemoryUserRepository>();
                services.AddScoped<IUserRepository>(sp =>
                { var dbContext = sp.GetRequiredService<IAppDbContext>(); return new PostgresUserRepository(dbContext);});

            }
            else
            {
                switch (storageSettings.DefaultStore)
                {
                    case "Mongo":
                        services.AddScoped<ITodoTaskRepository>(sp =>
                        { var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value; return new MongoTodoTaskRepository(settings);});
                        break;

                    case "Postgres":
                        services.AddScoped<ITodoTaskRepository>(sp =>
                        { var dbContext = sp.GetRequiredService<IAppDbContext>(); return new PostgresTodoTaskRepository(dbContext);});

                        services.AddScoped<IUserRepository>(sp =>
                        { var dbContext = sp.GetRequiredService<IAppDbContext>(); return new PostgresUserRepository(dbContext);});
                        break;

                    default: // InMemory
                        services.AddScoped<ITodoTaskRepository, InMemoryTodoTaskRepository>();
                        services.AddScoped<IUserRepository, InMemoryUserRepository>();
                        break;
                }
            }

            return services;
        }
    }
}