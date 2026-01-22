namespace EAITMApp.SharedKernel.Errors.Hooks
{
    /// <summary>
    /// Default no-operation implementation of <see cref="IErrorHook"/>.
    /// Used when no error hooks are registered to avoid null checks
    /// and conditional execution in the error handling pipeline.
    /// </summary>
    public sealed class NoOpErrorHook : IErrorHook
    {
        /// <inheritdoc/>
        public Task BeforeMapAsync(ErrorHookContext context) => Task.CompletedTask;

        /// <inheritdoc/>
        public Task AfterMapAsync(ErrorHookContext context) => Task.CompletedTask;

        /// <inheritdoc/>
        public Task OnCriticalAsync(ErrorHookContext context) => Task.CompletedTask;
    }
}
