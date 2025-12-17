using EAITMApp.Application.Common.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EAITMApp.Api.Controllers
{
    [ApiController]
    public abstract class ApiResponseController : ControllerBase
    {
        /// <summary>
        /// Success operation with data
        /// </summary>
        protected IActionResult Success<T>(T data, string message) where T : class
        {
            return Ok(ApiResponse<T>.Success(data, message));
        }

        /// <summary>
        /// Success operation without data
        /// </summary>
        protected IActionResult Success(string message)
        {
            return Ok(ApiResponse<object>.Success(null, message));
        }
    }
}
