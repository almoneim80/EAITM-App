using EAITMApp.SharedKernel.Errors;
using EAITMApp.SharedKernel.Errors.Contracts;
using EAITMApp.SharedKernel.Exceptions;

namespace EAITMApp.Infrastructure.Errors
{
    public class BaseAppExceptionMapper : IErrorMapper
    {
        public int Priority => 0;
        public bool CanMap(Exception exception) => exception is BaseAppException;
        public async Task<ErrorMappingResult> MapAsync(Exception exception, ErrorContext context)
        {
            var ex = (BaseAppException)exception;
            var descriptor = ex.Descriptor;

            var apiError = new ApiError(
                Code: descriptor.Code,
                Message: descriptor.IsSafeToExpose ? ex.Message : descriptor.DefaultMessage,
                TraceId: context.TraceId,
                Severity: descriptor.Severity
            );

            var result = new ErrorMappingResult(descriptor.HttpStatus, apiError);
            return await Task.FromResult(result);
        }
    }
}
