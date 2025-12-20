namespace EAITMApp.SharedKernel.Errors
{
    public sealed record ErrorContext
    (
        string TraceId,
        string RequestId,
        string Environment,
        DateTimeOffset Timestamp,
        IReadOnlyDictionary<string, object>? Metadata = null
    );
}
