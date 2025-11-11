namespace EAITMApp.Infrastructure.Repositories.Settings
{
    public class DataStoresSettings
    {
        public IDatabaseConnectionSettings Write { get; set; } = new DatabaseConnectionSettings();
        public IDatabaseConnectionSettings Read { get; set; } = new DatabaseConnectionSettings();
    }
}
