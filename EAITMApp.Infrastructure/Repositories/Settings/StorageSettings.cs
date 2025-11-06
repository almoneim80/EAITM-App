namespace EAITMApp.Infrastructure.Repositories.Settings
{
    public class StorageSettings
    {
        public bool EnableMultipleStores { get; set; }
        public string DefaultStore { get; set; } = "InMemory";
    }
}
