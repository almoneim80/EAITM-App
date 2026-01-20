using EAITMApp.SharedKernel.Errors;

namespace EAITMApp.SharedKernel.Exceptions
{
    /// <summary>
    /// The base type for infrastructure-related exceptions.
    /// Represents failures originating from external systems, frameworks,
    /// or runtime dependencies.
    /// </summary>
    public class InfrastructureExceptionBase : BaseAppException
    {
        protected InfrastructureExceptionBase(ErrorDescriptor descriptor, IDictionary<string, object>? metadata = null, Exception? innerException = null)
            : base(descriptor, metadata, innerException)
        { }
    }
}
