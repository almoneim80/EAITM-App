namespace EAITMApp.Application.Common.Responses
{
    public sealed class ApiResponse<T>
    {
        public bool IsSuccess { get; }
        public string Message { get; }
        public T? Data { get; }
        public IReadOnlyList<ApiError> Errors { get; }

        public ApiResponse(bool isSuccess, string message, T? data, IReadOnlyList<ApiError> errors)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
            Errors = errors;
        }

        public static ApiResponse<T> Success(T data, string message = "Operation completed successfully.")
        {
            return new ApiResponse<T>(
                true,
                message,
                data,
                Array.Empty<ApiError>());
        }

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
