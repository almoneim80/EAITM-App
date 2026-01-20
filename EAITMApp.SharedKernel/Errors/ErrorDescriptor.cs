using EAITMApp.SharedKernel.Errors.Enums;

namespace EAITMApp.SharedKernel.Errors
{
    /// <summary>
    /// Represents a complete, immutable definition of an application error,
    /// including its identity, classification, severity, HTTP mapping,
    /// and exposure rules.
    /// </summary>
    public sealed class ErrorDescriptor
    {
        /// <summary>
        /// A unique, stable identifier for the error used for tracking and diagnostics.
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// Indicates the high-level category that describes the source of the error.
        /// </summary>
        public ErrorCategory Category { get; }

        /// <summary>
        /// Specifies how critical or impactful the error is.
        /// </summary>
        public ErrorSeverity Severity { get; }

        /// <summary>
        /// The HTTP status code associated with this error.
        /// </summary>
        public int HttpStatus { get; }

        /// <summary>
        /// A default, human-readable message describing the error.
        /// </summary>
        public string DefaultMessage { get; }

        /// <summary>
        /// Determines whether the error details can be safely exposed to external clients.
        /// </summary>
        public bool IsSafeToExpose { get; }

        /// <summary>
        /// Initializes a new error descriptor with validated metadata
        /// used consistently across the application.
        /// </summary>
        public ErrorDescriptor(
            string code,
            ErrorCategory category,
            ErrorSeverity severity,
            AppHttpStatus httpStatus,
            string defaultMessage,
            bool isSafeToExpose = true)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Error code must be provided.", nameof(code));

            int status = (int)httpStatus;
            if (status < 100 || status > 599)
                throw new ArgumentOutOfRangeException(nameof(httpStatus),
                    "HttpStatus must be a valid HTTP status code (100-599).");

            Code = code;
            Category = category;
            Severity = severity;
            HttpStatus = status;
            DefaultMessage = defaultMessage;
            IsSafeToExpose = isSafeToExpose;
        }
    }
}
