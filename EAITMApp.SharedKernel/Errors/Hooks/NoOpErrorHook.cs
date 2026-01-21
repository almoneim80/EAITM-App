namespace EAITMApp.SharedKernel.Errors.Hooks
{
    public sealed class NoOpErrorHook : IErrorHook
    {
        public Task BeforeMapAsync(ErrorHookContext context) => Task.CompletedTask;
        public Task AfterMapAsync(ErrorHookContext context) => Task.CompletedTask;
        public Task OnCriticalAsync(ErrorHookContext context) => Task.CompletedTask;
    }
}
