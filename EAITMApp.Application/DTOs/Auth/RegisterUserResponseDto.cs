namespace EAITMApp.Application.DTOs.Auth
{
    /// <summary>
    /// Represents the data returned after successfully registering a new user.
    /// </summary>
    public class RegisterUserResponseDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the registered user.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the username of the registered user.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the email address of the registered user.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the role assigned to the registered user.
        /// </summary>
        public string Role { get; set; }
    }
}
