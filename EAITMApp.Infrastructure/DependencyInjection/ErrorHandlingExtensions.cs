using EAITMApp.Infrastructure.Context;
using EAITMApp.Infrastructure.Errors.Policies;
using EAITMApp.Infrastructure.Errors;
using EAITMApp.SharedKernel.Common;
using EAITMApp.SharedKernel.Errors;
using Microsoft.Extensions.DependencyInjection;

namespace EAITMApp.Infrastructure.DependencyInjection
{
    public static class ErrorHandlingExtensions
    {
        public static IServiceCollection AddGlobalErrorHandling(this IServiceCollection services, AppEnvironment environment)
        {
            // Register the Context Provider to generate error data (TraceId, Timestamp)
            services.AddHttpContextAccessor();
            services.AddScoped<IErrorContextProvider, ErrorContextProvider>();

            // Register the factory responsible for selecting the error detection policy according to the environment (Dev, Prod).
            services.AddSingleton(new ErrorExposurePolicyFactory(environment));

            // Register mappers to convert exceptions to ApiError
            // ValidationExceptionMapper is registered first because it has a higher priority (100)
            services.AddSingleton<IErrorMapper, ValidationExceptionMapper>();
            services.AddSingleton<IErrorMapper, BaseAppExceptionMapper>();

            // Register the main engine that manages the entire connection process.
            services.AddScoped<ErrorMappingEngine>();

            return services;
        }
    }
}
