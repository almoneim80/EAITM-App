using EAITMApp.Infrastructure.Errors;

namespace EAITMApp.Api.Middlewares
{
    /// <summary>
    /// Global middleware to handle unhandled exceptions in the request pipeline.
    /// Converts exceptions into standardized API error responses using <see cref="ErrorMappingEngine"/>.
    /// </summary>
    public class GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger = logger;


        /// <summary>
        /// Invokes the middleware for the current HTTP context.
        /// Catches unhandled exceptions, maps them using <see cref="ErrorMappingEngine"/>,
        /// and writes a consistent JSON response to the client.
        /// </summary>
        public async Task InvokeAsync(HttpContext context, ErrorMappingEngine engine)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                if (context.Response.HasStarted)
                {
                    // If the response has already started, we cannot modify the Status Code or write a new JSON.
                    // The only option is to log the error and let the exception propagate.
                    _logger.LogWarning("The response has already started. The middleware cannot convert the error into an ApiResponse.");
                    throw;
                }

                // Use the engine to convert the Exception.
                var result = await engine.MapExceptionAsync(ex);

                // Prepare the response.
                context.Response.StatusCode = result.StatusCode;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(result.Response);
            }
        }
    }
}
