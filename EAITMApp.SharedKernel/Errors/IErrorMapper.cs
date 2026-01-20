using EAITMApp.SharedKernel.Errors.Contracts;

namespace EAITMApp.SharedKernel.Errors
{
    /// <summary>
    /// Defines a contract for mapping exceptions to standardized <see cref="ApiError"/> representations.
    /// Implementations can apply custom logic to translate different exception types into structured API errors.
    /// </summary>
    public interface IErrorMapper
    {
        /// <summary>
        /// Determines the precedence of this mapper when multiple mappers can handle the same exception.
        /// Higher values indicate higher priority.
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// Checks whether this mapper can handle the provided exception type.
        /// </summary>
        bool CanMap(Exception exception);

        /// <summary>
        /// Maps the provided exception into an <see cref="ApiError"/> asynchronously,
        /// using contextual information such as trace ID, request ID, and environment.
        /// </summary>
        Task<ApiError> MapAsync(Exception exception, ErrorContext context);
    }
}
