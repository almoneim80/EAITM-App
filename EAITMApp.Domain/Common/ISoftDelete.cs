namespace EAITMApp.Domain.Common
{
    /// <summary>
    /// Represents an entity that supports soft deletion.
    /// Soft deletion allows marking an entity as deleted without physically removing it from the database,
    /// enabling recovery, auditing, and historical data analysis.
    /// </summary>
    public interface ISoftDelete
    {
        /// <summary>
        /// Indicates whether the entity has been soft deleted.
        /// </summary>
        bool IsDeleted { get; }

        /// <summary>
        /// The date and time when the entity was soft deleted.
        /// Nullable because the entity may never have been deleted.
        /// </summary>
        DateTimeOffset? DeletedAt { get; }

        /// <summary>
        /// Indicates whos do this delete operation.
        /// </summary>
        string? DeletedBy { get; }

        /// <summary>
        /// Optional expiration date for the soft deletion.
        /// After this date, the entity could be permanently purged if desired.
        /// </summary>
        DateTimeOffset? SoftDeleteExpiration { get; }
    }
}
