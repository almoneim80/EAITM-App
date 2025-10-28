using EAITMApp.Application.DTOs.Auth;
using EAITMApp.Application.UseCases.Commands.UserCMD;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EAITMApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UsersController> _logger;
        /// <summary>
        /// UsersController constructor.
        /// </summary>
        /// <param name="mediator">MediatR mediator for sending commands/queries.</param>
        /// <param name="logger">Logger instance for auditing and diagnostics.</param>
        public UsersController(IMediator mediator, ILogger<UsersController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        /// <param name="dto">The user data to register.</param>
        /// <returns>Returns the registered user data or error details.</returns>
        /// <response code="201">User successfully registered.</response>
        /// <response code="409">Username already exists.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var command = new RegisterUserCommand(dto);
                var result = await _mediator.Send(command);

                _logger.LogInformation("User {Username} registered successfully.", dto.Username);
                return CreatedAtAction(nameof(Register), new { id = result.Id }, result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Registration failed for {Username}: {Message}", dto.Username, ex.Message);
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during registration for {Username}", dto.Username);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Authenticates a user by validating username and password.
        /// </summary>
        /// <param name="dto">User login credentials.</param>
        /// <returns>Returns user data if valid, otherwise unauthorized.</returns>
        /// <response code="200">Login successful.</response>
        /// <response code="401">Invalid username or password.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var command = new LoginUserCommand(dto);
                var result = await _mediator.Send(command);

                _logger.LogInformation("User {Username} logged in successfully.", dto.Username);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning("Failed login for {Username}: {Message}", dto.Username, ex.Message);
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during login for {Username}", dto.Username);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }
    }
}
