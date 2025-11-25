namespace EAITMApp.Infrastructure.Repositories.Settings{
    /// <summary>    /// Represents the configuration for all data stores in the application,    /// including both the write (primary) and read (replica) databases.    /// Designed to be bindable via the Options pattern and strongly-typed.    /// </summary>    public class DataStoresSettings    {        /// <summary>
        /// Configuration for the primary (write) database.
        /// </summary>
        public DatabaseConnectionSettings WriteDatabaseSettings { get; set; } = new DatabaseConnectionSettings();

        /// <summary>
        /// Configuration for the read-only (replica) database.
        /// </summary>
        public DatabaseConnectionSettings ReadDatabaseSettings { get; set; } = new DatabaseConnectionSettings();    }}