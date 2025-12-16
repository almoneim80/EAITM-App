using EAITMApp.Application.Common.Responses;
using EAITMApp.Application.Exceptions;

namespace EAITMApp.Api.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (RequestValidationException ex)
            {
                // التعامل مع أخطاء التحقق فقط
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";

                var response = ApiResponse<object>.Failure(
                    "Validation failed",
                    ex.Errors);

                await context.Response.WriteAsJsonAsync(response);
            }
            catch(Exception ex)
            {
                // كل الاستثناءات الأخرى
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                var response = ApiResponse<object>.Failure(
                    "An unexpected error occurred. Please contact support.",
                    Array.Empty<ApiError>()
                    );

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
