namespace EAITMApp.SharedKernel.Errors
{
    /// <summary>
    /// Provides the current error context for the executing operation,
    /// enabling access to trace identifiers, request information, environment,
    /// and additional metadata for logging or error mapping.
    /// </summary>
    public interface IErrorContextProvider
    {
        /// <summary>
        /// Gets the <see cref="ErrorContext"/> associated with the current operation.
        /// </summary>
        ErrorContext Current { get; }
    }
}
