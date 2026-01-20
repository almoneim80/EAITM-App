namespace EAITMApp.SharedKernel.Errors.Enums
{
    /// <summary>
    /// Defines the severity level of an application error,
    /// used for logging, monitoring, and client response purposes.
    /// </summary>
    public enum ErrorSeverity
    {
        Low = 1,        // User mistake (NotFound, Validation)
        Medium = 2,     // Business rule violation
        High = 3,       // Security / Authorization
        Critical = 4   // System failure
    }
}
