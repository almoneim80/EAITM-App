namespace EAITMApp.SharedKernel.Common
{
    /// <summary>
    /// Represents the different runtime environments in which the application can operate.
    /// Used to select environment-specific behaviors, such as error handling policies.
    /// </summary>
    public enum AppEnvironment
    {
        Development,
        Production,
        Testing,
        Staging
    }
}