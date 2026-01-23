using EAITMApp.Application.Persistence;

namespace EAITMApp.Infrastructure.Settings
{
    /// <summary>
    /// Default implementation of <see cref="IDatabaseConnectionSettings"/>.
    /// Can be bound directly from configuration using the Options pattern.
    /// </summary>
    public class DatabaseConnectionSettings : IDatabaseConnectionSettings
    {
        /// <inheritdoc/>
        public string ProviderType { get; set; } = string.Empty;

        /// <inheritdoc/>
        public string Host { get; set; } = string.Empty;

        /// <inheritdoc/>
        public int Port { get; set; } = 0;

        /// <inheritdoc/>
        public string Database { get; set; } = string.Empty;

        /// <inheritdoc/>
        public string Username { get; set; } = string.Empty;

        /// <inheritdoc/>
        public string Password { get; set; } = string.Empty;

        /// <inheritdoc/>
        public string SslMode { get; set; } = string.Empty;

        /// <inheritdoc/>
        public bool Pooling { get; set; } = true;

        /// <inheritdoc/>
        public Dictionary<string, string> AdditionalSettings { get; set; } = new();
    }
}
