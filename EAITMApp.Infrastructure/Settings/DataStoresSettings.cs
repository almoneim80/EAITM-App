using EAITMApp.Application.Persistence;namespace EAITMApp.Infrastructure.Settings{
    /// <summary>    /// Represents the configuration for all data stores in the application,    /// including both the write (primary) and read (replica) databases.    /// Designed to be bindable via the Options pattern and strongly-typed.    /// </summary>    public class DataStoresSettings : IDataStoresSettings    {        /// <summary>
        /// Configuration for the primary (write) database.
        /// </summary>
        public IDatabaseConnectionSettings WriteDatabaseSettings { get; set; } = new DatabaseConnectionSettings();

        /// <summary>
        /// Configuration for the read-only (replica) database.
        /// </summary>
        public IDatabaseConnectionSettings ReadDatabaseSettings { get; set; } = new DatabaseConnectionSettings();    }}