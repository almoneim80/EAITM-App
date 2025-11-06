using EAITMApp.Application.DTOs.Auth;
using MediatR;

namespace EAITMApp.Application.UseCases.Commands.UserCMD
{
    /// <summary>
    /// Command to authenticate a user and generate login response.
    /// </summary>
    public class LoginUserCommand(LoginUserDto loginDto) : IRequest<LoginUserResponseDto>
    {
        public string Username => loginDto.Username;
        public string Password => loginDto.Password;
    }
}
