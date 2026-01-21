using EAITMApp.SharedKernel.Errors.Contracts;
using EAITMApp.SharedKernel.Errors.Policies;
using EAITMApp.SharedKernel.Errors;
using EAITMApp.SharedKernel.Exceptions;

namespace EAITMApp.Infrastructure.Errors.Policies
{
    /// <summary>
    /// Applies error exposure rules for testing environments.
    /// Provides controlled diagnostic metadata for testing purposes
    /// without exposing sensitive production information.
    /// </summary>
    public sealed class TestingErrorExposurePolicy : IErrorExposurePolicy
    {
        /// <inheritdoc/>
        public ApiError Apply(ApiError error, ErrorContext context, Exception exception)
        {
            return error with
            {
                Metadata = new Dictionary<string, object?>
                {
                    ["TestHint"] = "This is a controlled test environment error.",
                    ["ExceptionType"] = exception.GetType().Name,
                    ["TraceId"] = context.TraceId,
                    ["CanExpose"] = exception is BaseAppException be && be.Descriptor.IsSafeToExpose
                }
            };
        }
    }
}
