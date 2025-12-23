using EAITMApp.SharedKernel.Errors.Contracts;

namespace EAITMApp.SharedKernel.Errors
{
    public sealed record ErrorMappingResult(
        int StatusCode,
        ApiResponse<object> Respons
    );
}
