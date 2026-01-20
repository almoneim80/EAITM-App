namespace EAITMApp.SharedKernel.Errors.Contracts
{
    /// <summary>
    /// Standardized wrapper for API responses, encapsulating the result status,
    /// a human-readable message, optional data, and a collection of errors.
    /// </summary>
    public sealed class ApiResponse<T>
    {
        /// <summary>
        /// Indicates whether the operation succeeded.
        /// </summary>
        public bool IsSuccess { get; }

        /// <summary>
        /// Human-readable message describing the result of the operation.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Optional payload data returned by the operation.
        /// </summary>
        public T? Data { get; }

        /// <summary>
        /// Collection of errors associated with the operation, empty if <see cref="IsSuccess"/> is true.
        /// </summary>
        public IReadOnlyList<ApiError> Errors { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="ApiResponse{T}"/>.
        /// </summary>
        public ApiResponse(bool isSuccess, string message, T? data, IReadOnlyList<ApiError> errors)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
            Errors = errors;
        }

        /// <summary>
        /// Creates a successful response containing the provided data.
        /// </summary>
        public static ApiResponse<T> Success(T data, string message = "Operation completed successfully.")
        {
            return new ApiResponse<T>(
                true,
                message,
                data,
                Array.Empty<ApiError>());
        }

        /// <summary>
        /// Creates a failed response with the specified message and associated errors.
        /// </summary>
        public static ApiResponse<T> Failure(string message, IReadOnlyList<ApiError> errors)
        {
            return new ApiResponse<T>(
                false,
                message,
                default,
                errors);
        }
    }
}
