using MediatR;

namespace EAITMApp.Application.UseCases.Commands.UserCMD
{
    /// <summary>
    /// Command to register a new user in the system.
    /// This command carries the required data to create a User entity.
    /// It is handled by <see cref="RegisterUserHandler"/>.
    /// </summary>
    public record RegisterUserCommand : IRequest<Guid>
    {
        public string Username { get; init; }
        public string Email { get; init; }
        public string Password { get; init; }
        public string Role { get; init; } = "User";

        public RegisterUserCommand(string username, string email, string password, string role = "User")
        {
            Username = username;
            Email = email;
            Password = password;
            Role = role;
        }
    }
}
