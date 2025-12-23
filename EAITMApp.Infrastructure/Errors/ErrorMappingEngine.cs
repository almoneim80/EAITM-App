using EAITMApp.SharedKernel.Errors;
using EAITMApp.SharedKernel.Errors.Contracts;
using EAITMApp.SharedKernel.Errors.Registries;
using Microsoft.Extensions.Logging;

namespace EAITMApp.Infrastructure.Errors
{
    public class ErrorMappingEngine
    {
        private readonly IEnumerable<IErrorMapper> _mappers;
        private readonly IErrorContextProvider _contextProvider;
        private readonly ILogger<ErrorMappingEngine> _logger;
        public ErrorMappingEngine(IEnumerable<IErrorMapper> mappers, IErrorContextProvider contextProvider, ILogger<ErrorMappingEngine> logger)
        {
            _mappers = mappers.OrderByDescending(m => m.Priority);
            _contextProvider = contextProvider;
            _logger = logger;
        }

        public async Task<ErrorMappingResult> MapExceptionAsync(Exception exception)
        {
            var context = _contextProvider.Current;
            _logger.LogError(exception, "Unhandled Exception: {Message}. [TraceId: {TraceId}] Context: {@ErrorContext}",
                exception.Message, context.TraceId, context);
            var mapper = _mappers.FirstOrDefault(m => m.CanMap(exception));

            if (mapper != null)
            {
                return await mapper.MapAsync(exception, context);
            }

            return MapToFallback(exception, context);
        }

        private ErrorMappingResult MapToFallback(Exception exception, ErrorContext context)
        {
            var descriptor = CommonErrors.UnexpectedError;
            string message = context.Environment == "Development" ? $"[DEV ONLY] {exception.Message}" : descriptor.DefaultMessage;
            var apiError = new ApiError(descriptor.Code, message, context.TraceId, descriptor.Severity.ToString());

            return BuildResult(descriptor.HttpStatus, apiError);
        }

        private ErrorMappingResult BuildResult(int statusCode, ApiError error)
        {
            // هنا "المكان الوحيد" في كل النظام الذي يقرر كيف يظهر رد الخطأ
            return new ErrorMappingResult(
                statusCode,
                ApiResponse<object>.Failure(error.Message, new[] { error })
            );
        }

    }
}
