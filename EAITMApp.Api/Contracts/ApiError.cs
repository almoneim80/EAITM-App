using EAITMApp.SharedKernel.Errors.Enums;

namespace EAITMApp.Application.DTOs.Responses
{
    public sealed record ApiError(
        string Code,
        string Message,
        string? Property = null,
        string? TraceId = null,
        ErrorSeverity Severity = ErrorSeverity.Low
    );
}
