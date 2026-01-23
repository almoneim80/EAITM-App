namespace EAITMApp.Application.Persistence.Validation
{
    /// <summary>
    /// Validates database connection settings before application startup.
    /// </summary>
    public interface IDatabaseSettingsValidator
    {
        void Validate(IDataStoresSettings settings);
    }
}
