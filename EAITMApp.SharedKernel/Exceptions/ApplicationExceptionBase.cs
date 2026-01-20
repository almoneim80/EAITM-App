using EAITMApp.SharedKernel.Errors;
using EAITMApp.SharedKernel.Exceptions;

namespace EAITMApp.Application.Exceptions
{
    /// <summary>
    /// The base type for all application-layer exceptions.
    /// Encapsulates a predefined <see cref="ErrorDescriptor"/> and optional
    /// contextual metadata to enable consistent error handling and propagation
    /// from the application layer.
    /// </summary>
    public abstract class ApplicationExceptionBase : BaseAppException
    {
        protected ApplicationExceptionBase(ErrorDescriptor descriptor, IDictionary<string, object>? metadata = null, Exception? innerException = null)
            : base(descriptor, metadata, innerException)
        { }
    }
}