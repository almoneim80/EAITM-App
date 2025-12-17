using EAITMApp.Application.Common.Enums;
using EAITMApp.Application.Common.Responses;

namespace EAITMApp.Application.Exceptions
{
    public class GeneralException : Exception
    {
        public IReadOnlyList<ApiError> Errors { get; }

        public GeneralException(string message = "An unexpected error occurred.")
            : base(message)
        {
            Errors = new[]
            {
                new ApiError(
                    Code: "INTERNAL_SERVER_ERROR",
                    Message: message,
                    Severity: ErrorSeverity.Critical
                )
            };
        }
    }
}
