using EAITMApp.Application.Common.Enums;
using EAITMApp.Application.Common.Responses;

namespace EAITMApp.Application.Exceptions
{
    public class NotFoundException : Exception
    {
        public string ResourceName { get; }
        public object ResourceKey { get; }
        public IReadOnlyList<ApiError> Errors { get; }

        public NotFoundException(string resourceName, object resourceKey)
            : base($"{resourceName} with resourceName '{resourceKey}' was not found.")
        {
            ResourceName = resourceName;
            ResourceKey = resourceKey;
            Errors = new[]
{
                new ApiError(
                    Code: "NOT_FOUND",
                    Message: this.Message,
                    Severity: ErrorSeverity.Low
                )
            };
        }
    }
}
