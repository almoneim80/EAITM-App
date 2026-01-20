using EAITMApp.SharedKernel.Errors;
using EAITMApp.SharedKernel.Exceptions;

namespace EAITMApp.Domain.Exceptions
{
    /// <summary>
    /// The base type for all domain-layer exceptions.
    /// Represents violations of business rules or invalid domain states.
    /// </summary>
    public abstract class DomainExceptionBase : BaseAppException
    {
        protected DomainExceptionBase(ErrorDescriptor descriptor, IDictionary<string, object>? metadata = null, Exception? innerException = null)
            : base(descriptor, metadata, innerException) { }
    }
}
