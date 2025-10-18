using EAITMApp.Application.DTOs.Auth;
using EAITMApp.Application.Interfaces;
using EAITMApp.Application.UseCases.Commands.UserCMD;
using EAITMApp.Domain.Entities;
using MediatR;

namespace EAITMApp.Application.Handlers.UserHDL
{
    /// <summary>
    /// Handles the <see cref="RegisterUserCommand"/> to register a new user.
    /// Validates uniqueness of the username, creates a User entity,
    /// persists it via <see cref="IUserRepository"/>, and returns user data.
    /// </summary>
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, RegisterUserResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEncryptionService _encryptionService;

        public RegisterUserHandler(IUserRepository repository, IEncryptionService encryptionService)
        {
            _userRepository = repository;
            _encryptionService = encryptionService;
        }

        public async Task<RegisterUserResponseDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByUsernameAsync(request.Username);
            if (existingUser != null)
                throw new InvalidOperationException("Username already exists");

            var hashedPassword = _encryptionService.HashPassword(request.Password);
            var user = new User(request.Username, request.Email, hashedPassword, request.Role);

            await _userRepository.AddAsync(user);

            return new RegisterUserResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role
            };
        }
    }
}

//TODO مستقبلي: تشفير كلمة المرور قبل إنشاء الكائن User.
//TODO مستقبلي: إضافة Logging(مثلاً: تسجيل محاولة التسجيل).
//TODO مستقبلي: إضافة Validations إضافية (Email format، طول Password).