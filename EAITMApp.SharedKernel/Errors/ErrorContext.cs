namespace EAITMApp.SharedKernel.Errors
{
    /// <summary>
    /// Encapsulates contextual information associated with an error occurrence,
    /// used for correlation, diagnostics, and structured logging.
    /// </summary>
    public sealed record ErrorContext
    (
        /// <summary>
        /// Unique identifier used to correlate the error across logs and distributed traces.
        /// </summary>
        string TraceId,

        /// <summary>
        /// Identifier of the originating request.
        /// </summary>
        string RequestId,

        /// <summary>
        /// Name of the runtime environment where the error occurred.
        /// </summary>
        string Environment,

        /// <summary>
        /// Timestamp indicating when the error was captured.
        /// </summary>
        DateTimeOffset Timestamp,

        /// <summary>
        /// Optional structured metadata providing additional diagnostic details.
        /// </summary>
        IReadOnlyDictionary<string, object>? Metadata = null
    );
}
