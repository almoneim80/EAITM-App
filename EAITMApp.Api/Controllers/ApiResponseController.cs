using Microsoft.AspNetCore.Mvc;
using EAITMApp.SharedKernel.Errors.Contracts;

namespace EAITMApp.Api.Controllers
{
    /// <summary>
    /// Base class for all controllers, providing unified methods for sending successful responses.
    /// Relies on <see cref="ApiResponse"/> to ensure a consistent response format across the system.
    /// </summary>
    [ApiController]
    public abstract class ApiResponseController : ControllerBase
    {
        /// <summary>
        /// Standard successful response (200 OK) containing data and a message.
        /// </summary>
        protected IActionResult Success<T>(T data, string message = "Operation completed successfully.")
        {
            return Ok(ApiResponse<T>.Success(data, message));
        }

        /// <summary>
        /// Successful response (200 OK) without data, typically used for update or delete operations.
        /// </summary>
        protected IActionResult Success(string message)
        {
            return Ok(ApiResponse<object>.Success(null!, message));
        }

        /// <summary>
        /// Resource creation response (201 Created) that includes a location link to the created resource.
        /// Provides a fallback mechanism if link generation fails to ensure system stability.
        /// </summary>
        protected IActionResult Created<T>(T data, string message, string actionName, object? routeValues = null)
        {
            var response = ApiResponse<T>.Success(data, message);

            if (string.IsNullOrWhiteSpace(actionName) || routeValues is null)
            {
                return CreatedFallback(data, message);
            }

            try
            {
                // محاولة توليد Location Header بشكل معيارى (RESTful)
                return CreatedAtAction(actionName, routeValues, response);
            }
            catch
            {
                // شبكة أمان: إذا فشل توليد الرابط، أرسل البيانات مع كود 201 بدلاً من انهيار الطلب
                return CreatedFallback(data, message);
            }
        }

        /// <summary>
        /// Resource creation response (201 Created) using a direct URI.
        /// </summary>
        protected IActionResult Created<T>(T data, string message, string? locationUri = null)
        {
            var response = ApiResponse<T>.Success(data, message);

            if (string.IsNullOrWhiteSpace(locationUri))
            {
                return CreatedFallback(data, message);
            }

            try
            {
                return Created(locationUri, response);
            }
            catch
            {
                return CreatedFallback(data, message);
            }
        }

        /// <summary>
        /// Fallback method that returns a 201 status code when dynamic link generation is not possible.
        /// </summary>
        private IActionResult CreatedFallback<T>(T data, string message)
        {
            var response = ApiResponse<T>.Success(data, message);
            return StatusCode(StatusCodes.Status201Created, response);
        }
    }
}