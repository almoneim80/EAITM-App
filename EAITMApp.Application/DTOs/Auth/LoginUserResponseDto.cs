namespace EAITMApp.Application.DTOs.Auth
{
    /// <summary>
    /// Represents the response returned after a successful user login.
    /// Contains essential user information and optional authentication token.
    /// </summary>
    public class LoginUserResponseDto
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// The role assigned to the authenticated user.
        /// </summary>
        public string Role { get; set; } = string.Empty;

        /// <summary>
        /// Optional authentication token (e.g., JWT) if using token-based authentication.
        /// Nullable to indicate absence when not issued.
        /// </summary>
        public string? Token { get; set; }

        /// <summary>
        /// Optional refresh token for token renewal scenarios.
        /// Nullable to indicate absence when not issued.
        /// </summary>
        public string? RefreshToken { get; set; }
    }
}
