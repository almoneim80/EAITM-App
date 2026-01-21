namespace EAITMApp.SharedKernel.Errors.Hooks
{
    public interface IErrorHook
    {
        Task BeforeMapAsync(ErrorHookContext context);
        Task AfterMapAsync(ErrorHookContext context);
        Task OnCriticalAsync(ErrorHookContext context);
    }
}
