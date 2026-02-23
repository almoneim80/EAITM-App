namespace EAITMApp.Domain.Common
{
    /// <summary>
    /// Represents an entity that tracks the last modification timestamp.
    /// Useful for auditing, detecting changes, and synchronizing data.
    /// </summary>
    public interface IHasUpdatedAt
    {
        /// <summary>
        /// Date and time when the entity was last updated.
        /// Nullable to indicate that the entity may not have been modified since creation.
        /// Stored as <see cref="DateTimeOffset"/> to preserve timezone information.
        /// </summary>
        DateTimeOffset? UpdatedAt { get; }
    }
}
