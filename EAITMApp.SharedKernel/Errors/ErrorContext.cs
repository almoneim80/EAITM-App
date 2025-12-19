namespace EAITMApp.SharedKernel.Errors
{
    public sealed class ErrorContext
    {
        public string TraceId { get; }
        public string RequestId { get; }
        public string Path { get; }
        public string Environment { get; }
        public DateTimeOffset Timestamp { get; }

        public ErrorContext(string traceId, string requestId, string path, string environment)
        {
            TraceId = traceId ?? throw new ArgumentNullException(nameof(traceId));
            RequestId = requestId ?? throw new ArgumentNullException(nameof(requestId));
            Path = path ?? throw new ArgumentNullException(nameof(path));
            Environment = environment ?? throw new ArgumentNullException(nameof(environment));
            Timestamp = DateTimeOffset.UtcNow;
        }
    }
}
