using EAITMApp.SharedKernel.Errors.Enums;

namespace EAITMApp.SharedKernel.Errors.Registries
{
    /// <summary>
    /// Defines task-related business error descriptors used to represent
    /// invalid or conflicting operations within the task domain.
    /// </summary>
    public class TaskErrors
    {
        private const string Prefix = "TASK ERROR: ";

        public static readonly ErrorDescriptor NotFound = new(
            code: "TASK.NOT_FOUND",
            category: ErrorCategory.Business,
            severity: ErrorSeverity.Low,
            httpStatus: AppHttpStatus.NotFound,
            defaultMessage: $"{Prefix} Task not found.",
            isSafeToExpose: true);

        public static readonly ErrorDescriptor AlreadyCompleted = new(
            code: "TASK.ALREADY_COMPLETED",
            category: ErrorCategory.Business,
            severity: ErrorSeverity.Medium,
            httpStatus: AppHttpStatus.Conflict,
            defaultMessage: $"{Prefix} Task is already completed.",
            isSafeToExpose: true);

        public static readonly ErrorDescriptor AlreadyExists = new(
            code: "TASKS.TITLE_ALREADY_EXISTS",
            category: ErrorCategory.Business,
            severity: ErrorSeverity.Medium,
            httpStatus: AppHttpStatus.Conflict,
            defaultMessage: $"{Prefix} Task title already exists.",
            isSafeToExpose: true);
    }
}