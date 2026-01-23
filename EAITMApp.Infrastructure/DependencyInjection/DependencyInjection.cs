using EAITMApp.Application.Interfaces;
using EAITMApp.Infrastructure.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EAITMApp.Infrastructure.Repositories.TaskRepo;
using EAITMApp.Infrastructure.Repositories.UserRepo;
using EAITMApp.Application.Persistence;
using EAITMApp.Infrastructure.Settings;
using EAITMApp.Infrastructure.Behaviors;
using MediatR;
using EAITMApp.Infrastructure.Errors;
using EAITMApp.SharedKernel.Errors;
using EAITMApp.Application.Behaviors;
using EAITMApp.Application.Persistence.Validation;
using EAITMApp.Infrastructure.Persistence.Validation;

namespace EAITMApp.Infrastructure.DependencyInjection
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
            // configure databases
            // =========================
            DatabaseRegistration.ConfigureDatabases(services, dataStores);

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

            // ===============================
            // Repository registration logic
            // ===============================
            services.AddScoped<IReadTodoTaskRepository>(sp =>
            {
                var dbContext = sp.GetRequiredService<IReadDbContext>();
                return new TodoTaskReadRepository(dbContext);
            });

            services.AddScoped<IWriteTodoTaskRepository>(sp =>
            {
                var dbContext = sp.GetRequiredService<IWriteDbContext>();
                return new TodoTaskWriteRepository(dbContext);
            });

            services.AddScoped<IUserRepository>(sp =>
            {
                var dbContext = sp.GetRequiredService<IWriteDbContext>();
                return new UserRepository(dbContext);
            });

            // =====================================
            // MediatR Pipeline Behaviors Registration
            // =====================================
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

            // =====================================
            // Exception Mapping Engine Registration
            // =====================================
            services.AddScoped<ErrorMappingEngine>();
            services.AddSingleton<IErrorMapper, BaseAppExceptionMapper>();
            
            return services;
        }
    }
}
