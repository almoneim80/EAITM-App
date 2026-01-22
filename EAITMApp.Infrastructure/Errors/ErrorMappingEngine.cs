using EAITMApp.Infrastructure.Errors.Policies;
using EAITMApp.SharedKernel.Errors;
using EAITMApp.SharedKernel.Errors.Contracts;
using EAITMApp.SharedKernel.Errors.Enums;
using EAITMApp.SharedKernel.Errors.Hooks;
using EAITMApp.SharedKernel.Errors.Policies;
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
        private readonly IErrorExposurePolicy _policy;
        private readonly IEnumerable<IErrorHook> _hooks;

        /// <summary>
        /// Initializes a new instance of <see cref="ErrorMappingEngine"/>.
        /// </summary>
        /// <param name="mappers">Collection of registered error mappers, ordered by priority.</param>
        public ErrorMappingEngine(
            IEnumerable<IErrorMapper> mappers, 
            IErrorContextProvider contextProvider, 
            ILogger<ErrorMappingEngine> logger,
            ErrorExposurePolicyFactory policyFactory,
            IEnumerable<IErrorHook> hooks)
        {
            // Sort the mappers once during injection to improve performance.
            _mappers = mappers.OrderByDescending(m => m.Priority);
            _contextProvider = contextProvider;
            _logger = logger;
            _policy = policyFactory.CreateErrorExposurePolicy();
            _hooks = hooks ?? Enumerable.Empty<IErrorHook>();
        }

        /// <summary>
        /// Maps an exception to a standardized <see cref="ErrorMappingResult"/>.
        /// Logs the exception along with context and applies the first applicable mapper.
        /// Falls back to a generic system error if no mapper matches.
        /// </summary>
        public async Task<ErrorMappingResult> MapExceptionAsync(Exception exception)
        {
            // get error context.
            var context = _contextProvider.Current;
            var hookContext = new ErrorHookContext(exception, context);

            foreach (var hook in _hooks) await hook.BeforeMapAsync(hookContext);

            _logger.LogError(exception, "Unhandled Exception: {Message}. [TraceId: {TraceId}] Context: {@ErrorContext}",
                exception.Message, context.TraceId, context);

            // FirstOrDefault will select the most specialized mapper (highest Priority) first
            var mapper = _mappers.FirstOrDefault(m => m.CanMap(exception));
            ApiError apiError;
            int statusCode;

            if (mapper != null)
            {
                // Retrieve data from the mapper
                apiError = await mapper.MapAsync(exception, context);
                statusCode = exception is BaseAppException baseEx ? baseEx.Descriptor.HttpStatus : 500;
            }
            else
            {
                var descriptor = CommonErrors.UnexpectedError;
                apiError = MapToFallback(exception, context);
                statusCode = descriptor.HttpStatus;
            }

            // تطبيق سياسة التطهير قبل التغليف النهائي
            var safeError = _policy.Apply(apiError, context, exception);
            var finalHookContext = hookContext with { ApiError = safeError, HttpStatusCode = statusCode };

            foreach (var hook in _hooks) await hook.AfterMapAsync(finalHookContext);
            if (safeError.Severity == ErrorSeverity.Critical)
            {
                foreach (var hook in _hooks) await hook.OnCriticalAsync(finalHookContext);
            }

            // Unified packaging of the response.
            return BuildResult(statusCode, safeError);
        }

        #region private helper methods
        /// <summary>
        /// Maps exceptions for which no specific mapper exists to a generic system error.
        /// In Development, includes the original exception message for diagnostics.
        /// </summary>
        private ApiError MapToFallback(Exception exception, ErrorContext context)
        {
            var descriptor = CommonErrors.UnexpectedError;
            string message = context.Environment == "Development" ? $"[DEV ONLY] {exception.Message}" : descriptor.DefaultMessage;
            return new ApiError(
                    Code: descriptor.Code,
                    Message: message,
                    Property: null,
                    TraceId: context.TraceId,
                    Severity: descriptor.Severity);
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
                ApiResponse<object>.Failure(error.Message, new[] { error }));
        }
        #endregion 
    }
}
