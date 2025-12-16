using EAITMApp.Application.Common.Responses;
using Microsoft.AspNetCore.Mvc;

namespace EAITMApp.Api.Controllers
{
    [ApiController]
    public abstract class ApiResponseController : ControllerBase
    {
        protected IActionResult Success<T>(T data, string message) where T : class
        {
            return Ok(ApiResponse<T>.Success(data, message));
        }

        protected IActionResult Success(string message)
        {
            return Ok(ApiResponse<object>.Success(null, message));
        }
    }
}
