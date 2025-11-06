namespace EAITMApp.Application.Interfaces
{
    /// <summary>
    /// Defines contract for password hashing and verification.
    /// </summary>
    public interface IEncryptionService
    {
        /// <summary>
        /// Hashes a plain text password.
        /// </summary>
        /// <param name="plainText">The password in plain text.</param>
        /// <returns>Hashed password string.</returns>
        string HashPassword(string plainText);

        /// <summary>
        /// Verifies that a plain text password matches the hashed password.
        /// </summary>
        /// <param name="plainText">The password in plain text.</param>
        /// <param name="hashedPassword">The stored hashed password.</param>
        /// <returns>True if match, false otherwise.</returns>
        bool VerifyPassword(string plainText, string hashedPassword);
    }
}
