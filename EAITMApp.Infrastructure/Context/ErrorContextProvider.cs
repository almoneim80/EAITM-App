using EAITMApp.SharedKernel.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Collections.ObjectModel;

namespace EAITMApp.Infrastructure.Context
{
    /// <summary>
    /// Provides the current <see cref="ErrorContext"/> for the executing HTTP request.
    /// Generates context including trace identifiers, environment, timestamp, and request metadata.
    /// </summary>
    public class ErrorContextProvider : IErrorContextProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHostEnvironment _env;
        private ErrorContext? _cachedContext;

        /// <summary>
        /// Initializes a new instance of <see cref="ErrorContextProvider"/>.
        /// </summary>
        public ErrorContextProvider(IHttpContextAccessor httpContextAccessor, IHostEnvironment env)
        {
            _httpContextAccessor = httpContextAccessor;
            _env = env;
        }

        /// <inheritdoc/>
        public ErrorContext Current => _cachedContext ??= GenerateContext();

        /// <summary>
        /// Generates a new <see cref="ErrorContext"/> based on the current HTTP request and environment.
        /// </summary>
        private ErrorContext GenerateContext()
        {
            var context = _httpContextAccessor.HttpContext;
            var traceId = context?.TraceIdentifier ?? Guid.NewGuid().ToString("N");
            var metadata = new ReadOnlyDictionary<string, object>(new Dictionary<string, object>
            {
                { "Transport", "HTTP" },
                { "Path", context?.Request.Path.Value ?? "N/A" },
                { "Method", context?.Request.Method ?? "N/A" }
            });

            return new ErrorContext(
                TraceId: traceId,
                RequestId: traceId,
                Environment: _env.EnvironmentName,
                Timestamp: DateTimeOffset.UtcNow,
                Metadata: metadata);
        }
    }
}
