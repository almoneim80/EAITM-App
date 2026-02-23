namespace EAITMApp.Domain.Common
{
    public interface IAuditableEntity : IHasCreatedAt, IHasUpdatedAt
    {
        string? CreatedBy { get; }
        string? UpdatedBy { get; }

        void SetCreation(DateTimeOffset createdAt, string? createdBy);
        void SetModification(DateTimeOffset updatedAt, string? updatedBy);
    }
}
