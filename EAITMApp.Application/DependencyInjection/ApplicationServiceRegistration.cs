using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EAITMApp.Application.DependencyInjection
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // 1. تسجيل MediatR
            // يتم تسجيل جميع الـ Handlers و الـ Behaviors (باستثناء الذي في Infrastructure)
            services.AddMediatR(cfg =>
            {
                // نحدد أن Handlers موجودة في الـ Assembly الحالي (Application)
                cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            });

            // 2. تسجيل FluentValidation
            // تسجيل جميع الـ Validators التي ترث من AbstractValidator في طبقة التطبيق.
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // 3. (اختياري) تسجيل خدمات أخرى خاصة بالتطبيق (مثل Mappers)
            // services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
