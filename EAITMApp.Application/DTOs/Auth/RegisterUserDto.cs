namespace EAITMApp.Application.DTOs.Auth
{
    /// <summary>
    /// Data required to register a new user.
    /// Used as input for <see cref="RegisterUserCommand"/>.
    /// </summary>
    public class RegisterUserDto
    {
        /// <summary>
        /// Gets or sets the username of the new user.
        /// </summary>
        public string Username { get; init; } = default!;

        /// <summary>
        /// Gets or sets the email address of the new user.
        /// </summary>
        public string Email { get; init; } = default!;

        /// <summary>
        /// Gets or sets the password of the new user.
        /// </summary>
        public string Password { get; init; } = default!;
    }
}