namespace EAITMApp.Application.Persistence
{
    public interface IDataStoresSettings
    {
        IDatabaseConnectionSettings WriteDatabaseSettings { get; }
        IDatabaseConnectionSettings ReadDatabaseSettings { get; }
    }
}
