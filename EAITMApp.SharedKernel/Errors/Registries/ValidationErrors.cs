using EAITMApp.SharedKernel.Errors.Enums;

namespace EAITMApp.SharedKernel.Errors.Registries
{
    public static class ValidationErrors
    {
        private const string Prefix = "VALIDATION ERROR: ";

        public static readonly ErrorDescriptor General = new(
            code: "VALIDATION.GENERAL_FAILURE",
            category: ErrorCategory.Validation,
            severity: ErrorSeverity.Low,
            httpStatus: AppHttpStatus.BadRequest,
            defaultMessage: $"{Prefix} Validation failed.",
            isSafeToExpose: true);

        public static readonly ErrorDescriptor Required = new(
            code: "VALIDATION.REQUIRED",
            category: ErrorCategory.Validation,
            severity: ErrorSeverity.Low,
            httpStatus: AppHttpStatus.BadRequest,
            defaultMessage: $"{Prefix} This field is required.",
            isSafeToExpose: true);

        public static readonly ErrorDescriptor InvalidFormat = new(
            code: "VALIDATION.INVALID_FORMAT",
            category: ErrorCategory.Validation,
            severity: ErrorSeverity.Low,
            httpStatus: AppHttpStatus.BadRequest,
            defaultMessage: $"{Prefix} Invalid format.",
            isSafeToExpose: true);

        public static readonly ErrorDescriptor ValidationFailed = new(
            code: "VALIDATION.FAILED",
            category: ErrorCategory.Validation,
            severity: ErrorSeverity.Low,
            httpStatus: AppHttpStatus.BadRequest,
            defaultMessage: $"{Prefix} Validation failed for one or more fields.",
            isSafeToExpose: true
            );
    }
}
