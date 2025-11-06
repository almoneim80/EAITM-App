using EAITMApp.Application.DTOs.Auth;
using FluentValidation;

namespace EAITMApp.Application.Validators
{
    /// <summary>
    /// Validates <see cref="LoginUserDto"/> for login requests.
    /// Ensures required fields are present and meet basic constraints.
    /// </summary>
    public class LoginUserDtoValidator : AbstractValidator<LoginUserDto>
    {
        public LoginUserDtoValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required.")
                .MaximumLength(50).WithMessage("Username must not exceed 50 characters.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
                .MaximumLength(100).WithMessage("Password must not exceed 100 characters.");
        }
    }
}
