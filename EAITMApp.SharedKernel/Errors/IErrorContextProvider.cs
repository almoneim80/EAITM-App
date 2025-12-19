namespace EAITMApp.SharedKernel.Errors
{
    public interface IErrorContextProvider
    {
        ErrorContext Current { get; }
    }
}
