using EAITMApp.SharedKernel.Errors.Contracts;

namespace EAITMApp.SharedKernel.Errors.Hooks
{
    /// <summary>
    /// Provides contextual data passed to error hooks during error handling.
    /// Acts as a shared container for the exception, mapping context,
    /// and optional results produced during the error pipeline.
    /// </summary>
    public sealed record ErrorHookContext(
        Exception Exception,
        ErrorContext ErrorContext,
        ApiError? ApiError = null,
        int? HttpStatusCode = null
    );
}
