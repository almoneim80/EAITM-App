namespace EAITMApp.Domain.Common
{
    /// <summary>
    /// Represents an entity that tracks the creation timestamp.
    /// Useful for auditing, sorting, and analyzing the timeline of entities.
    /// </summary>
    public interface IHasCreatedAt
    {
        /// <summary>
        /// Date and time when the entity was created.
        /// Stored as <see cref="DateTimeOffset"/> to preserve timezone information.
        /// </summary>
        DateTimeOffset CreatedAt { get; }
    }
}
