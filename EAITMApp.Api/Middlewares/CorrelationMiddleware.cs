namespace EAITMApp.Api.Middlewares
{
    public class CorrelationMiddleware
    {
        private readonly RequestDelegate _next;
        private const string CorrelationHeaderKey = "X-Correlation-ID";
        public CorrelationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // 1. استخراج الـ ID من الطلب أو استخدام TraceIdentifier الجاهز
            if (!context.Request.Headers.TryGetValue(CorrelationHeaderKey, out var correlationId) || string.IsNullOrWhiteSpace(correlationId))
            {
                correlationId = context.TraceIdentifier ?? Guid.NewGuid().ToString();
            }

            // 2. مزامنة الـ TraceIdentifier لضمان أن الـ Context Provider سيسحب نفس القيمة
            context.TraceIdentifier = correlationId;

            // 3. إضافة الـ ID إلى رأس الاستجابة (Response Header)
            context.Response.OnStarting(() =>
            {
                if (!context.Response.Headers.ContainsKey(CorrelationHeaderKey))
                {
                    context.Response.Headers.Append(CorrelationHeaderKey, correlationId);
                }

                return Task.CompletedTask;
            });

            await _next(context);
        }
    }
}
