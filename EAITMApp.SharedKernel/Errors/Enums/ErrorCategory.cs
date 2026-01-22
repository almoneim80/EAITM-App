namespace EAITMApp.SharedKernel.Errors.Enums
{
    /// <summary>
    /// Defines high-level categories for classifying application errors
    /// based on their source and responsibility (validation, business rules,
    /// security, infrastructure, or system-level failures).
    /// </summary>
    public enum ErrorCategory
    {
        Validation = 1,        // Input errors 
        Business = 2,          // Business rules errors
        Security = 3,          // Permissions and access errors
        Infrastructure = 4,    // External failure (database, other server)
        System = 5,             // Unexpected errors
        Request = 6,
    }
}
