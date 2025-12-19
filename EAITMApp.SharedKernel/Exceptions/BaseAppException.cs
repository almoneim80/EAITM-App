using EAITMApp.SharedKernel.Errors;

namespace EAITMApp.SharedKernel.Exceptions
{
    public abstract class BaseAppException : Exception
    {
        /// <summary>
        /// Descriptor ثابت يحدد هوية الخطأ
        /// </summary>
        public ErrorDescriptor Descriptor { get; }


        /// <summary>
        /// بيانات إضافية قابلة للتوسعة
        /// </summary>
        public IReadOnlyDictionary<string, object> Metadata { get; }

        /// <summary>
        /// وقت حدوث الاستثناء
        /// </summary>
        public DateTimeOffset Timestamp { get; }

        protected BaseAppException(ErrorDescriptor descriptor, IDictionary<string, object>? metadata = null, Exception? innerException = null)
            : base(descriptor.DefaultMessage, innerException)
        {
            Descriptor = descriptor ?? throw new ArgumentNullException(nameof(descriptor));
            Metadata = metadata != null ? new Dictionary<string, object>(metadata) : new Dictionary<string, object>();
            Timestamp = DateTimeOffset.UtcNow;
        }
    }
}
