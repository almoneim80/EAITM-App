using EAITMApp.Application.DTOs.Auth;
using MediatR;

namespace EAITMApp.Application.UseCases.Commands.UserCMD
{
    /// <summary>
    /// Command to register a new user in the system.
    /// This command carries the required data to create a User entity.
    /// It is handled by <see cref="RegisterUserHandler"/>.
    /// </summary>
    public record RegisterUserCommand(RegisterUserDto UserDto) : IRequest<RegisterUserResponseDto>
    {
        public string Username => UserDto.Username;
        public string Email => UserDto.Email;
        public string Password => UserDto.Password;
        public string Role { get; init; } = "User";
    }
}
