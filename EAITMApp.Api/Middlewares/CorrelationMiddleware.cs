namespace EAITMApp.Api.Middlewares
{
    /// <summary>
    /// Middleware that ensures each HTTP request has a correlation identifier (X-Correlation-ID)
    /// to enable distributed tracing, logging, and consistent error context propagation.
    /// </summary>
    public class CorrelationMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Header key used to store the correlation ID.
        /// </summary>
        private const string CorrelationHeaderKey = "X-Correlation-ID";
        public CorrelationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Invokes the middleware logic to assign or propagate a correlation ID.
        /// </summary>
        public async Task InvokeAsync(HttpContext context)
        {
            // Extract correlation ID from request header or fallback to TraceIdentifier
            if (!context.Request.Headers.TryGetValue(CorrelationHeaderKey, out var correlationId) || string.IsNullOrWhiteSpace(correlationId))
            {
                correlationId = context.TraceIdentifier ?? Guid.NewGuid().ToString();
            }

            // Synchronize TraceIdentifier so ErrorContextProvider retrieves the same value
            context.TraceIdentifier = correlationId!;

            // Add correlation ID to response headers if not already present
            context.Response.OnStarting(() =>
            {
                if (!context.Response.Headers.ContainsKey(CorrelationHeaderKey))
                {
                    context.Response.Headers.Append(CorrelationHeaderKey, correlationId);
                }

                return Task.CompletedTask;
            });

            // Call the next middleware in the pipeline
            await _next(context);
        }
    }
}
