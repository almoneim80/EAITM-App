using EAITMApp.SharedKernel.Errors.Enums;

namespace EAITMApp.SharedKernel.Errors.DTOs
{
    public sealed record ValidationFailureDto(
        string PropertyPath,
        string Message,
        string RuleCode,
        ErrorSeverity Severity
    );
}
