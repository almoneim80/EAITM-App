using EAITMApp.SharedKernel.Errors.Enums;

namespace EAITMApp.SharedKernel.Validation
{
    /// <summary>
    /// Represents a single validation failure resulting from a rule violation.
    /// Encapsulates the rule that failed, the message, the property path, and the severity level.
    /// </summary>
    public sealed record ValidationFailure(
        string RuleCode,
        string Message,
        string PropertyPath,
        ErrorSeverity Severity = ErrorSeverity.Low
    );
}