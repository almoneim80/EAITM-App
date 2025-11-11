namespace EAITMApp.Infrastructure.Repositories.Settings
{
    public class DatabaseConnectionSettings : IDatabaseConnectionSettings
    {
        public string Type { get; set; } = "Postgres";
        public Dictionary<string, object> Options { get; set; } = new();
    }
}
