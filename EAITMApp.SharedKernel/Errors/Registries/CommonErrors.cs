using EAITMApp.SharedKernel.Errors.Enums;

namespace EAITMApp.SharedKernel.Errors.Registries
{
    /// <summary>
    /// Provides a centralized registry of common, cross-cutting error descriptors
    /// used across the system for infrastructure and system-level failures.
    /// </summary>
    public static class CommonErrors
    {
        private const string CodePrefix = "GEN"; // General
        private const string SystemMessagePrefix = "SYSTEM ERROR: ";
        private const string RequestMessagePrefix = "REQUEST ERROR: ";

        public static readonly ErrorDescriptor UnexpectedError = new(
            code: $"{CodePrefix}.UNEXPECTED_ERROR",
            category: ErrorCategory.System,
            httpStatus: AppHttpStatus.InternalServerError,
            severity: ErrorSeverity.Critical,
            defaultMessage: $"{SystemMessagePrefix} An unexpected error occurred on our server.",
            isSafeToExpose: false);

        public static readonly ErrorDescriptor Unknown = new(
            code: $"{CodePrefix}.UNKNOWN_ERROR",
            category: ErrorCategory.System,
            severity: ErrorSeverity.Critical,
            httpStatus: AppHttpStatus.InternalServerError,
            defaultMessage: $"{SystemMessagePrefix} An unknown error occurred.",
            isSafeToExpose: false);

        public static readonly ErrorDescriptor RequestTimeout = new(
            code: $"{CodePrefix}.REQUEST_TIMEOUT",
            category: ErrorCategory.Infrastructure,
            severity: ErrorSeverity.Medium,
            httpStatus: AppHttpStatus.InternalServerError,
            defaultMessage: $"{SystemMessagePrefix}The request timed out.",
            isSafeToExpose: true);

        public static readonly ErrorDescriptor ServiceUnavailable = new(
            code: $"{CodePrefix}.SERVICE_UNAVAILABLE",
            category: ErrorCategory.Infrastructure,
            severity: ErrorSeverity.High,
            httpStatus: AppHttpStatus.InternalServerError,
            defaultMessage: $"{SystemMessagePrefix} The service is temporarily down for maintenance.",
            isSafeToExpose: true);

        public static readonly ErrorDescriptor IdMismatch = new(
            code: $"{CodePrefix}.ID_MISMATCH",
            category: ErrorCategory.Request,
            severity: ErrorSeverity.Low,
            httpStatus: AppHttpStatus.BadRequest, // 400
            defaultMessage: $"{RequestMessagePrefix}The ID in the URL does not match the ID in the request body.",
            isSafeToExpose: true
    );
    }
}
