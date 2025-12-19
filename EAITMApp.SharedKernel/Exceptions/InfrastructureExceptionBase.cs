using EAITMApp.SharedKernel.Errors;

namespace EAITMApp.SharedKernel.Exceptions
{
    public class InfrastructureExceptionBase : BaseAppException
    {
        protected InfrastructureExceptionBase(ErrorDescriptor descriptor, IDictionary<string, object>? metadata = null, Exception? innerException = null)
            : base(descriptor, metadata, innerException)
        { }
    }
}
