using EAITMApp.SharedKernel.Errors.Contracts;
using EAITMApp.SharedKernel.Errors.DTOs;
using EAITMApp.SharedKernel.Errors.Enums;
using EAITMApp.SharedKernel.Errors.Registries;
using EAITMApp.SharedKernel.Exceptions;

namespace EAITMApp.SharedKernel.Errors
{
    public sealed class ValidationExceptionMapper : IErrorMapper
    {
        public int Priority => 100; // أعلى من BaseAppException

        public bool CanMap(Exception exception)
            => exception is ValidationException;

        public Task<ApiError> MapAsync(Exception exception, ErrorContext context)
        {
            var ex = (ValidationException)exception;
            // تحويل كل ValidationFailure إلى DTO
            var failureDtos = ex.Failures
                .Select(f => new ValidationFailureDto(
                    f.PropertyPath,
                    f.Message,
                    f.RuleCode,
                    f.Severity
                ))
                .ToList();

            // تحديد Severity الكلي للـ ApiError بناءً على أقوى فشل
            var overallSeverity = failureDtos.Any()
                ? failureDtos.Max(f => f.Severity)
                : ErrorSeverity.Low;

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
