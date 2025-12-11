namespace EAITMApp.Application.Persistence
{
    /// <summary>
    /// Represents the settings required to connect to a database.
    /// This interface enforces a strongly-typed configuration instead of using a generic dictionary.
    /// </summary>
    public interface IDatabaseConnectionSettings
    {
        /// <summary>
        /// The type of the database provider (e.g., "Postgres", "SqlServer").
        /// </summary>
        string ProviderType { get; set; }

        /// <summary>
        /// The host or IP address of the database server.
        /// </summary>
        string Host { get; set; }

        /// <summary>
        /// The port on which the database server is listening.
        /// </summary>
        int Port { get; set; }

        /// <summary>
        /// The database name.
        /// </summary>
        string Database { get; set; }

        /// <summary>
        /// The username for authentication.
        /// </summary>
        string Username { get; set; }

        /// <summary>
        /// The password for authentication.
        /// </summary>
        string Password { get; set; }

        /// <summary>
        /// The SSL mode for the connection.
        /// </summary>
        Npgsql.SslMode SslMode { get; set; }

        /// <summary>
        /// The pooling behavior for the connection.
        /// </summary>
        bool Pooling { get; set; }

        /// <summary>
        /// Optional additional settings specific to the provider.
        /// </summary>
        Dictionary<string, string> AdditionalSettings { get; set; }
    }
}
