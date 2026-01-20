using EAITMApp.SharedKernel.Errors;
using EAITMApp.SharedKernel.Errors.Contracts;
using EAITMApp.SharedKernel.Exceptions;

namespace EAITMApp.Infrastructure.Errors
{
    /// <summary>
    /// Maps <see cref="BaseAppException"/> instances to <see cref="ApiError"/> objects.
    /// Ensures that only errors marked as safe to expose reveal their original message,
    /// while others fallback to the descriptor's default message.
    /// </summary>
    public class BaseAppExceptionMapper : IErrorMapper
    {
        /// <inheritdoc/>
        public int Priority => 0; // Law priority becuase its general mapper.

        /// <inheritdoc/>
        public bool CanMap(Exception exception) => exception is BaseAppException;

        /// <inheritdoc/>
        public async Task<ApiError> MapAsync(Exception exception, ErrorContext context)
        {
            var ex = (BaseAppException)exception;
            var descriptor = ex.Descriptor;

            var apiError = new ApiError(
                Code: descriptor.Code,
                Message: descriptor.IsSafeToExpose ? ex.Message : descriptor.DefaultMessage,
                TraceId: context.TraceId,
                Severity: descriptor.Severity
            );

            return await Task.FromResult(apiError);
        }
    }
}
