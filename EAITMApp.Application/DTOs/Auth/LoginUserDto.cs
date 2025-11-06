namespace EAITMApp.Application.DTOs.Auth
{
    /// <summary>
    /// Data Transfer Object (DTO) for user login requests.
    /// Contains the credentials required to authenticate a user.
    /// </summary>
    public class LoginUserDto
    {
        /// <summary>
        /// Gets or sets the username of the user attempting to log in.
        /// </summary>
        /// <example>john.doe</example>
        public string Username { get; set; } = default!;

        /// <summary>
        /// Gets or sets the plain-text password of the user.
        /// This will be verified against the stored hashed password.
        /// </summary>
        /// <example>P@ssw0rd!</example>
        public string Password { get; set; } = default!;
    }
}
