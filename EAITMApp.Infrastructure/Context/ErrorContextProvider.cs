using EAITMApp.SharedKernel.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Collections.ObjectModel;

namespace EAITMApp.Infrastructure.Context
{
    public class ErrorContextProvider : IErrorContextProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHostEnvironment _env;
        private ErrorContext? _cachedContext;

        public ErrorContextProvider(IHttpContextAccessor httpContextAccessor, IHostEnvironment env)
        {
            _httpContextAccessor = httpContextAccessor;
            _env = env;
        }

        public ErrorContext Current => _cachedContext ??= GenerateContext();
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
