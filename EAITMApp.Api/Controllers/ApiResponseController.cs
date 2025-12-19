using EAITMApp.Application.DTOs.Responses;
using Microsoft.AspNetCore.Mvc;

namespace EAITMApp.Api.Controllers
{
    [ApiController]
    public abstract class ApiResponseController : ControllerBase
    {
        /// <summary>
        /// Success operation with data
        /// </summary>
        protected IActionResult Success<T>(T data, string message)
        {
            return Ok(ApiResponse<T>.Success(data, message));
        }

        /// <summary>
        /// Success operation without data.
        /// </summary>
        protected IActionResult Success(string message)
        {
            return Ok(ApiResponse<object>.Success(null, message));
        }

        /// <summary>
        /// 
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
                return CreatedAtAction(actionName, routeValues, response);
            }
            catch
            {
                return CreatedFallback(data, message);
            }
        }

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

        private IActionResult CreatedFallback<T>(T data, string message)
        {
            var response = ApiResponse<T>.Success(data, message);
            return StatusCode(StatusCodes.Status201Created, response);
        }
    }
}
