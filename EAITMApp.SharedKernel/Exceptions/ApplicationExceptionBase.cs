using EAITMApp.SharedKernel.Errors;
using EAITMApp.SharedKernel.Exceptions;

namespace EAITMApp.Application.Exceptions
{
    public abstract class ApplicationExceptionBase : BaseAppException
    {
        protected ApplicationExceptionBase(ErrorDescriptor descriptor, IDictionary<string, object>? metadata = null, Exception? innerException = null)
            : base(descriptor, metadata, innerException)
        { }
    }
}
