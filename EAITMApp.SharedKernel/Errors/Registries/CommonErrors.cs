using EAITMApp.SharedKernel.Errors.Enums;

namespace EAITMApp.SharedKernel.Errors.Registries
{
    public static class CommonErrors
    {
        private const string Prefix = "SYSTEM ERROR: ";

        public static readonly ErrorDescriptor UnexpectedError = new(
            code: "COMMON.UNEXPECTED_ERROR",
            category: ErrorCategory.System,
            httpStatus: AppHttpStatus.InternalServerError,
            severity: ErrorSeverity.Critical,
            defaultMessage: $"{Prefix} An unexpected error occurred on our server.",
            isSafeToExpose: false);

        public static readonly ErrorDescriptor Unknown = new(
            code: "UNKNOWN_ERROR",
            category: ErrorCategory.System,
            severity: ErrorSeverity.Critical,
            httpStatus: AppHttpStatus.InternalServerError,
            defaultMessage: $"{Prefix} An unknown error occurred.",
            isSafeToExpose: false);

        public static readonly ErrorDescriptor RequestTimeout = new(
            code: "COMMON.REQUEST_TIMEOUT",
            category: ErrorCategory.Infrastructure,
            severity: ErrorSeverity.Medium,
            httpStatus: AppHttpStatus.InternalServerError,
            defaultMessage: $"{Prefix} The request took too long to process.",
            isSafeToExpose: true);

        public static readonly ErrorDescriptor ServiceUnavailable = new(
            code: "COMMON.SERVICE_UNAVAILABLE",
            category: ErrorCategory.Infrastructure,
            severity: ErrorSeverity.High,
            httpStatus: AppHttpStatus.InternalServerError,
            defaultMessage: $"{Prefix} The service is temporarily down for maintenance.",
            isSafeToExpose: true);
    }
}
