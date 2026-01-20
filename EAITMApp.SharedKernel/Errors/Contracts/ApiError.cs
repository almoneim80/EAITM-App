using EAITMApp.SharedKernel.Errors.Enums;

namespace EAITMApp.SharedKernel.Errors.Contracts
{
    /// <summary>
    /// Represents a single error in an API response, including a unique code, message,
    /// optional property reference, trace identifier, and severity level.
    /// </summary>
    public sealed record ApiError(
        string Code,
        string Message,
        string? Property = null,
        string? TraceId = null,
        ErrorSeverity Severity = ErrorSeverity.Low
    );
}
