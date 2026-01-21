using EAITMApp.SharedKernel.Errors.Contracts;
using EAITMApp.SharedKernel.Errors.DTOs;
using EAITMApp.SharedKernel.Errors.Enums;
using EAITMApp.SharedKernel.Errors.Registries;
using EAITMApp.SharedKernel.Exceptions;

namespace EAITMApp.SharedKernel.Errors
{
    /// <summary>
    /// Maps validation exceptions to a standardized API error response.
    /// Converts validation failures into structured metadata consumable by clients.
    /// </summary>
    public sealed class ValidationExceptionMapper : IErrorMapper
    {
        /// <inheritdoc/>
        public int Priority => 100; // Higher than BaseAppException

        /// <inheritdoc/>
        public bool CanMap(Exception exception) => exception is ValidationException;

        /// <inheritdoc/>
        public Task<ApiError> MapAsync(Exception exception, ErrorContext context)
        {
            var ex = (ValidationException)exception;
            var failureDtos = ex.Failures.Select(f => new ValidationFailureDto(
                    f.PropertyPath,
                    f.Message,
                    f.RuleCode,
                    f.Severity
                )).ToList();

            // Determine the overall Severity of the ApiError based on the most severe failure
            var overallSeverity = failureDtos.Any() ? failureDtos.Max(f => f.Severity) : ErrorSeverity.Low;

            var apiError = new ApiError(
                Code: ValidationErrors.ValidationFailed.Code,
                Message: ValidationErrors.ValidationFailed.DefaultMessage,
                TraceId: context.TraceId,
                Severity: overallSeverity,
                Metadata: new Dictionary<string, object?>
                {
                    ["Failures"] = failureDtos
                }
            );

            return Task.FromResult(apiError);
        }
    }
}
