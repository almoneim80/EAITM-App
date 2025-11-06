using EAITMApp.Application.DTOs.Auth;
using EAITMApp.Application.Interfaces;
using EAITMApp.Application.UseCases.Commands.UserCMD;
using MediatR;

namespace EAITMApp.Application.Handlers.UserHDL
{
    /// <summary>
    /// Handles user login by validating credentials and returning user data.
    /// </summary>
    public class LoginUserHandler: IRequestHandler<LoginUserCommand, LoginUserResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEncryptionService _encryptionService;

        public LoginUserHandler(IUserRepository repository, IEncryptionService encryptionService)
        {
            _userRepository = repository;
            _encryptionService = encryptionService;
        }

        public async Task<LoginUserResponseDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUsernameAsync(request.Username);
            if (user == null)
                throw new InvalidOperationException("Invalid username or password");

            if (!_encryptionService.VerifyPassword(request.Password, user.PasswordHash))
                throw new InvalidOperationException("Invalid username or password");

            return new LoginUserResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                Token = null,
                RefreshToken = null
            };
        }
    }
}
