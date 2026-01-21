using EAITMApp.SharedKernel.Errors.Enums;

namespace EAITMApp.SharedKernel.Validation
{
    public sealed record ValidationFailure(
        string RuleCode,
        string Message,
        string PropertyPath,
        ErrorSeverity Severity = ErrorSeverity.Low
    );
}