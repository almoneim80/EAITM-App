using EAITMApp.Application.Common.Enums;

namespace EAITMApp.Application.Common.Responses
{
    public sealed record ApiError(
        string Code,
        string Message,
        string? Property = null,
        string? TraceId = null,
        ErrorSeverity Severity = ErrorSeverity.Low
    );
}
