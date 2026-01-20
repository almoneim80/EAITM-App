using EAITMApp.SharedKernel.Errors.Contracts;

namespace EAITMApp.SharedKernel.Errors
{
    /// <summary>
    /// Represents the result of mapping an exception to a standardized API error response.
    /// Contains the HTTP status code and the structured <see cref="ApiResponse{T}"/> payload.
    /// </summary>
    public sealed record ErrorMappingResult(
        int StatusCode,
        ApiResponse<object> Response
    );
}
