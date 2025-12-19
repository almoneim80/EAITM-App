namespace EAITMApp.SharedKernel.Errors.Enums
{
    public enum ErrorSeverity
    {
        Low = 1,        // User mistake (NotFound, Validation)
        Medium = 2,     // Business rule violation
        High = 3,       // Security / Authorization
        Critical = 4   // System failure
    }
}
