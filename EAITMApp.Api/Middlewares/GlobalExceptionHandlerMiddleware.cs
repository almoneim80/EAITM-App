using EAITMApp.Infrastructure.Errors;

namespace EAITMApp.Api.Middlewares
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;
        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

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
                    // إذا بدأ الإرسال فعلاً، لا يمكننا تغيير الـ Status Code أو كتابة JSON جديد
                    // الحل الوحيد هو تسجيل الخطأ في الـ Log وترك الاستثناء يرتفع للأعلى
                    _logger.LogWarning("الرد بدأ بالفعل. لا يمكن للميدلوير تحويل الخطأ لـ ApiResponse.");
                    throw;
                }

                // 2. استخدام المحرك لتحويل الـ Exception إلى نتيجة معتمدة
                var result = await engine.MapExceptionAsync(ex);

                // 4. إعداد الرد للمستخدم
                context.Response.StatusCode = result.StatusCode;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(result.Respons);
            }
        }
    }
}
