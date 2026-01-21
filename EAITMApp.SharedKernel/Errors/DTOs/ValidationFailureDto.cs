using EAITMApp.SharedKernel.Errors.Enums;

namespace EAITMApp.SharedKernel.Errors.DTOs
{
    /// <summary>
    /// Represents a single validation failure returned as part of a validation error response.
    /// </summary>
    public sealed record ValidationFailureDto(
        string PropertyPath,
        string Message,
        string RuleCode,
        ErrorSeverity Severity
    );
}
