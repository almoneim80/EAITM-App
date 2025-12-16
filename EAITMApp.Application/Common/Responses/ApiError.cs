namespace EAITMApp.Application.Common.Responses
{
    public sealed record ApiError(
        string Code,
        string Message,
        string? Property = null
    );
}
