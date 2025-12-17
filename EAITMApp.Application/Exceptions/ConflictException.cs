using EAITMApp.Application.Common.Enums;
using EAITMApp.Application.Common.Responses;

namespace EAITMApp.Application.Exceptions
{
    public class ConflictException : Exception
    {
        public IReadOnlyList<ApiError> Errors { get; }
        public ConflictException(string message, IReadOnlyList<ApiError>? errors = null)
            : base(message)
        {
            Errors = errors ?? new[]
            {
                new ApiError(
                    Code: "CONFLICT",
                    Message: message,
                    Severity: ErrorSeverity.Medium
                    )
            };
        }
    }
}
