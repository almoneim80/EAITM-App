using EAITMApp.SharedKernel.Errors;
using EAITMApp.SharedKernel.Errors.Contracts;
using EAITMApp.SharedKernel.Errors.Policies;

namespace EAITMApp.Infrastructure.Errors.Policies
{
    /// <summary>
    /// Applies error exposure rules for development environments.
    /// Provides detailed diagnostic information including exception message,
    /// stack trace, inner exception, context metadata, and source for debugging purposes.
    /// </summary>
    public sealed class DevelopmentErrorExposurePolicy : IErrorExposurePolicy
    {
        /// <inheritdoc/>
        public ApiError Apply(ApiError error, ErrorContext context, Exception exception)
        {
            return error with
            {
                Metadata = new Dictionary<string, object?> 
                {
                    {"ExceptionMessage", exception.Message },
                    { "StackTrace", exception.StackTrace },
                    { "InnerException", exception.InnerException?.Message },
                    { "ContextData", context.Metadata },
                    { "Source", exception.Source }
                }
            };
        }
    }
}
