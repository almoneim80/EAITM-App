using EAITMApp.SharedKernel.Errors.Contracts;
using EAITMApp.SharedKernel.Errors;
using EAITMApp.SharedKernel.Errors.Policies;
using EAITMApp.SharedKernel.Exceptions;
using EAITMApp.SharedKernel.Errors.Registries;
using EAITMApp.SharedKernel.Errors.Enums;

namespace EAITMApp.Infrastructure.Errors.Policies
{
    /// <summary>
    /// Applies error exposure rules for production environments.
    /// Ensures that sensitive or critical error details are not exposed to clients,
    /// replacing them with generic messages and clearing metadata as needed.
    /// </summary>
    public sealed class ProductionErrorExposurePolicy : IErrorExposurePolicy
    {
        /// <inheritdoc/>
        public ApiError Apply(ApiError error, ErrorContext context, Exception exception)
        {
            bool isSafe = error.Severity != ErrorSeverity.Critical 
                && (exception is BaseAppException be && be.Descriptor.IsSafeToExpose);
            return error with
            {
                Message = isSafe ? error.Message : CommonErrors.UnexpectedError.DefaultMessage,
                Metadata = null
            };
        }
    }
}
