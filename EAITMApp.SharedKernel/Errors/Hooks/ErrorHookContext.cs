using EAITMApp.SharedKernel.Errors.Contracts;

namespace EAITMApp.SharedKernel.Errors.Hooks
{
    public sealed record ErrorHookContext(
        Exception Exception,
        ErrorContext ErrorContext,
        ApiError? ApiError = null,
        int? HttpStatusCode = null
    );
}
