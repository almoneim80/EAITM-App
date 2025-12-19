using EAITMApp.SharedKernel.Errors.Enums;

namespace EAITMApp.SharedKernel.Errors
{
    public sealed class ErrorDescriptor
    {
        public string Code { get; }
        public ErrorCategory Category { get; }
        public ErrorSeverity Severity { get; }
        public int HttpStatus { get; }
        public string DefaultMessage { get; }
        public bool IsSafeToExpose { get; }

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
