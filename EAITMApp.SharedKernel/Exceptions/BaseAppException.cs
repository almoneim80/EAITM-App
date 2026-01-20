using EAITMApp.SharedKernel.Errors;

namespace EAITMApp.SharedKernel.Exceptions
{
    /// <summary>
    /// Serves as the base class for all application-specific exceptions,
    /// providing structured error information, extensible metadata,
    /// and consistent error identity.
    /// </summary>
    public abstract class BaseAppException : Exception
    {
        /// <summary>
        /// Describes the error in a structured and consistent way,
        /// including its code, category, severity, and HTTP mapping.
        /// </summary>
        public ErrorDescriptor Descriptor { get; }

        /// <summary>
        /// Additional contextual data related to the error,
        /// intended for diagnostics and logging.
        /// </summary>
        public IReadOnlyDictionary<string, object> Metadata { get; }

        /// <summary>
        /// The UTC timestamp indicating when the exception was created.
        /// </summary>
        public DateTimeOffset Timestamp { get; }

        /// <summary>
        /// Initializes the base application exception using a predefined error descriptor,
        /// optional metadata, and an optional inner exception.
        /// </summary>
        protected BaseAppException(ErrorDescriptor descriptor, IDictionary<string, object>? metadata = null, Exception? innerException = null)
            : base(descriptor.DefaultMessage, innerException)
        {
            Descriptor = descriptor ?? throw new ArgumentNullException(nameof(descriptor));
            Metadata = metadata != null ? new Dictionary<string, object>(metadata) : new Dictionary<string, object>();
            Timestamp = DateTimeOffset.UtcNow;
        }
    }
}
