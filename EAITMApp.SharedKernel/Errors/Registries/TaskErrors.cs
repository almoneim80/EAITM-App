using EAITMApp.SharedKernel.Errors.Enums;

namespace EAITMApp.SharedKernel.Errors.Registries
{
    /// <summary>
    /// Defines task-related business error descriptors used to represent
    /// invalid or conflicting operations within the task domain.
    /// </summary>
    public class TaskErrors
    {
        private const string CodePrefix = "TASK";
        private const string MessagePrefix = "TASK ERROR: ";

        public static ErrorDescriptor NotFound(Guid? id = null) => new(
            code: $"{CodePrefix}.NOT_FOUND",
            category: ErrorCategory.Business,
            severity: ErrorSeverity.Low,
            httpStatus: AppHttpStatus.NotFound,
            defaultMessage: id.HasValue
                ? $"{MessagePrefix}Task with ID '{id}' was not found."
                : $"{MessagePrefix}The requested task was not found.",
            isSafeToExpose: true);

        public static readonly ErrorDescriptor AlreadyCompleted = new(
            code: $"{CodePrefix}.ALREADY_COMPLETED",
            category: ErrorCategory.Business,
            severity: ErrorSeverity.Medium,
            httpStatus: AppHttpStatus.Conflict,
            defaultMessage: $"{MessagePrefix} Task is already completed.",
            isSafeToExpose: true);

        public static readonly ErrorDescriptor AlreadyExists = new(
            code: $"{CodePrefix}.TITLE_ALREADY_EXISTS",
            category: ErrorCategory.Business,
            severity: ErrorSeverity.Medium,
            httpStatus: AppHttpStatus.Conflict,
            defaultMessage: $"{MessagePrefix} Task title already exists.",
            isSafeToExpose: true);

        public static readonly ErrorDescriptor DuplicateTitle = new(
            code: $"{CodePrefix}.DUPLICATE_TITLE",
            category: ErrorCategory.Business,
            severity: ErrorSeverity.Medium,
            httpStatus: AppHttpStatus.Conflict,
            defaultMessage: $"{MessagePrefix} A task with the same title already exists. Please use a unique title.",
            isSafeToExpose: true);
    }
}