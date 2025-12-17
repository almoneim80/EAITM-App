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
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";

                var errors = ex.Errors.Select(e => e with { TraceId = context.TraceIdentifier}).ToArray();
                var response = ApiResponse<object>.Failure("Validation failed",errors);

                await context.Response.WriteAsJsonAsync(response);
            }
            catch (ConflictException ex)
            {
                context.Response.StatusCode = StatusCodes.Status409Conflict;
                context.Response.ContentType = "application/json";

                var errors = ex.Errors.Select(e => e with { TraceId = context.TraceIdentifier }).ToArray();
                var response = ApiResponse<object>.Failure("Conflict occurred while processing the request", errors);

                await context.Response.WriteAsJsonAsync(response);
            }
            catch (NotFoundException ex)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                context.Response.ContentType = "application/json";

                var errors = ex.Errors.Select(e => e with { TraceId = context.TraceIdentifier }).ToArray();
                var response = ApiResponse<object>.Failure("Resource not found", errors);

                await context.Response.WriteAsJsonAsync(response);
            }
            catch (GeneralException ex)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                var errors = ex.Errors.Select(e => e with { TraceId = context.TraceIdentifier }).ToArray();
                var response = ApiResponse<object>.Failure("An unexpected error occurred.", errors);

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
