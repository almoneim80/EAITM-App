namespace EAITMApp.Infrastructure.Security
{
    /// <summary>
    /// Represents configuration settings for Argon2 password hashing.
    /// Values are loaded from appsettings.json via IOptions&lt;Argon2Settings&gt;.
    /// </summary>
    public class Argon2Settings
    {
        /// <summary>
        /// Size of the salt in bytes (recommended 16 bytes / 128 bits).
        /// </summary>
        public int SaltSize { get; set; } = 16;

        /// <summary>
        /// Size of the resulting hash in bytes (recommended 32 bytes / 256 bits).
        /// </summary>
        public int HashSize { get; set; } = 32;

        /// <summary>
        /// Number of iterations (higher = slower but more secure).
        /// </summary>
        public int Iterations { get; set; } = 4;

        /// <summary>
        /// Memory cost in KB (higher = slower but more resistant to attacks).
        /// </summary>
        public int MemoryCost { get; set; } = 65536;

        /// <summary>
        /// Degree of parallelism (number of threads used during hashing).
        /// </summary>
        public int DegreeOfParallelism { get; set; } = 2;

        /// <summary>
        /// Argon2 type: Argon2d, Argon2i, or Argon2id.
        /// </summary>
        public string Type { get; set; } = "Argon2id";

        /// <summary>
        /// Optional associated data for Argon2.
        /// </summary>
        public string AssociatedData { get; set; } = "";

        /// <summary>
        /// Optional secret key for Argon2.
        /// </summary>
        public string SecretKey { get; set; } = "";
    }
}
