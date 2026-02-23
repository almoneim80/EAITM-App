namespace EAITMApp.Application.Interfaces
{
    public interface ICurrentUserService
    {
        string? UserId { get; }
        string? IpAddress { get; }
        string? UserAgent { get; }
    }
}
