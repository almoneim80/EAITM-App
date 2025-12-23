namespace EAITMApp.SharedKernel.Errors
{
    public interface IErrorMapper
    {
        int Priority { get; }
        bool CanMap(Exception exception);
        Task<ErrorMappingResult> MapAsync(Exception exception, ErrorContext context);
    }
}
