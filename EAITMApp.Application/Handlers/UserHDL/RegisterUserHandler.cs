using EAITMApp.Application.Interfaces;
using EAITMApp.Application.UseCases.Commands.UserCMD;
using EAITMApp.Domain.Entities;
using MediatR;

namespace EAITMApp.Application.Handlers.UserHDL
{
    /// <summary>
    /// Handles the <see cref="RegisterUserCommand"/> to register a new user.
    /// Validates that the username is unique, creates a User entity,
    /// and persists it through the <see cref="IUserRepository"/>.
    /// Returns the generated <see cref="User.Id"/>.
    /// </summary>
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, Guid>
    {
        private readonly IUserRepository _userRepository;

        public RegisterUserHandler(IUserRepository repository)
        {
            _userRepository = repository;
        }

        public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByUsernameAsync(request.Username);
            if (existingUser != null)
                throw new InvalidOperationException("Username already exists");

            var user = new User(request.Username, request.Email, request.Password, request.Role);

            await _userRepository.AddAsync(user);

            return user.Id;
        }
    }
}

//TODO مستقبلي: تشفير كلمة المرور قبل إنشاء الكائن User.
//TODO مستقبلي: إضافة Logging(مثلاً: تسجيل محاولة التسجيل).
//TODO مستقبلي: إضافة Validations إضافية (Email format، طول Password).
//إرجاع النتائج للـ API (مثلاً Id أو بيانات المستخدم المسجل)، من الأفضل استخدام DTO للـ Response