using EAITMApp.SharedKernel.Errors;
using EAITMApp.SharedKernel.Exceptions;

namespace EAITMApp.Domain.Exceptions
{
    public abstract class DomainExceptionBase : BaseAppException
    {
        protected DomainExceptionBase(ErrorDescriptor descriptor, IDictionary<string, object>? metadata = null, Exception? innerException = null)
            : base(descriptor, metadata, innerException) { }
    }
}
