using EAITMApp.SharedKernel.Errors;
using EAITMApp.SharedKernel.Errors.Contracts;
using EAITMApp.SharedKernel.Errors.Registries;
using EAITMApp.SharedKernel.Exceptions;
using Microsoft.Extensions.Logging;

namespace EAITMApp.Infrastructure.Errors
{
    /// <summary>
    /// Responsible for mapping exceptions to standardized <see cref="ErrorMappingResult"/> instances.
    /// Orchestrates registered <see cref="IErrorMapper"/> implementations, logs the error context,
    /// and ensures a consistent API response structure for all unhandled exceptions.
    /// </summary>
    public class ErrorMappingEngine
    {
        private readonly IEnumerable<IErrorMapper> _mappers;
        private readonly IErrorContextProvider _contextProvider;
        private readonly ILogger<ErrorMappingEngine> _logger;

        /// <summary>
        /// Initializes a new instance of <see cref="ErrorMappingEngine"/>.
        /// </summary>
        /// <param name="mappers">Collection of registered error mappers, ordered by priority.</param>
        public ErrorMappingEngine(IEnumerable<IErrorMapper> mappers, IErrorContextProvider contextProvider, ILogger<ErrorMappingEngine> logger)
        {
            // Sort the mappers once during injection to improve performance.
            _mappers = mappers.OrderByDescending(m => m.Priority);
            _contextProvider = contextProvider;
            _logger = logger;
        }

        /// <summary>
        /// Maps an exception to a standardized <see cref="ErrorMappingResult"/>.
        /// Logs the exception along with context and applies the first applicable mapper.
        /// Falls back to a generic system error if no mapper matches.
        /// </summary>
        public async Task<ErrorMappingResult> MapExceptionAsync(Exception exception)
        {
            var context = _contextProvider.Current;
            _logger.LogError(exception, "Unhandled Exception: {Message}. [TraceId: {TraceId}] Context: {@ErrorContext}",
                exception.Message, context.TraceId, context);

            // FirstOrDefault will select the most specialized mapper (highest Priority) first
            var mapper = _mappers.FirstOrDefault(m => m.CanMap(exception));

            if (mapper != null)
            {
                // Retrieve data from the mapper
                var apiError = await mapper.MapAsync(exception, context);
                int statusCode = exception is BaseAppException baseEx ? baseEx.Descriptor.HttpStatus : 500;

                // Unified packaging of the response.
                return BuildResult(statusCode, apiError);
            }

            return MapToFallback(exception, context);
        }

        /// <summary>
        /// Maps exceptions for which no specific mapper exists to a generic system error.
        /// In Development, includes the original exception message for diagnostics.
        /// </summary>
        private ErrorMappingResult MapToFallback(Exception exception, ErrorContext context)
        {
            var descriptor = CommonErrors.UnexpectedError;
            string message = context.Environment == "Development" ? $"[DEV ONLY] {exception.Message}" : descriptor.DefaultMessage;
            var apiError = new ApiError(descriptor.Code, message, context.TraceId, descriptor.Severity.ToString());

            return BuildResult(descriptor.HttpStatus, apiError);
        }

        /// <summary>
        /// Constructs a single, consistent <see cref="ErrorMappingResult"/> for API responses.
        /// This is the single source of truth for formatting error outputs.
        /// </summary>
        private ErrorMappingResult BuildResult(int statusCode, ApiError error)
        {
            // This is the single place in the entire system that determines how the error response is presented.
            return new ErrorMappingResult(
                statusCode,
                ApiResponse<object>.Failure(error.Message, new[] { error })
            );
        }

    }
}
