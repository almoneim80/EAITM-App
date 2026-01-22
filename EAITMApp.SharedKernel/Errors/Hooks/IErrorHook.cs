namespace EAITMApp.SharedKernel.Errors.Hooks
{
    /// <summary>
    /// Defines extensibility hooks executed at specific stages
    /// of the error handling and mapping pipeline.
    /// </summary>
    public interface IErrorHook
    {
        /// <summary>
        /// Invoked before an exception is mapped to an API error.
        /// Allows inspection or enrichment of the hook context.
        /// </summary>
        Task BeforeMapAsync(ErrorHookContext context);

        /// <summary>
        /// Defines extensibility hooks executed at specific stages
        /// of the error handling and mapping pipeline.
        /// </summary>
        Task AfterMapAsync(ErrorHookContext context);

        /// <summary>
        /// Invoked when a critical error is detected.
        /// Intended for high-severity actions such as alerts or incident reporting.
        /// </summary>
        Task OnCriticalAsync(ErrorHookContext context);
    }
}
