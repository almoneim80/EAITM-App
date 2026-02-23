namespace EAITMApp.Domain.Common
{
    /// <summary>
    /// Base class for entities that support soft deletion and audit timestamps.
    /// Inherits a generic identifier from <see cref="BaseEntityWithId{TKey}"/> 
    /// and implements created/updated timestamps and soft deletion tracking.
    /// </summary>
    public abstract class BaseSoftDeletableEntity<TKey> 
        : BaseEntityWithId<TKey>, IAuditableEntity, ISoftDelete where TKey : IEquatable<TKey>
    {
        /// <inheritdoc/>
        public DateTimeOffset CreatedAt { get; protected set; }

        /// <inheritdoc/>
        public string? CreatedBy { get; protected set; }

        /// <inheritdoc/>
        public DateTimeOffset? UpdatedAt { get; protected set; }

        /// <inheritdoc/>
        public string? UpdatedBy { get; protected set; }

        /// <inheritdoc/>
        public bool IsDeleted { get; protected set; }

        /// <inheritdoc/>
        public DateTimeOffset? DeletedAt { get; protected set; }

        /// <inheritdoc/>
        public string? DeletedBy { get; protected set; }

        /// <inheritdoc/>
        public DateTimeOffset? SoftDeleteExpiration { get; protected set; }

        /// <inheritdoc/>
        public void SetCreation(DateTimeOffset createdAt, string? createdBy)
        {
            CreatedAt = createdAt;
            CreatedBy = createdBy;
        }

        /// <inheritdoc/>
        public void SetModification(DateTimeOffset updatedAt, string? updatedBy)
        {
            UpdatedAt = updatedAt;
            UpdatedBy = updatedBy;
        }

        public virtual void SoftDelete(string? deletedBy)
        {
            var now = DateTimeOffset.UtcNow;
            IsDeleted = true;
            DeletedAt = now;
            DeletedBy = deletedBy;
            UpdatedAt = now;
            UpdatedBy = deletedBy;
        }

        public virtual void UndoDelete()
        {
            var now = DateTimeOffset.UtcNow;
            IsDeleted = false;
            DeletedAt = null;
            DeletedBy = null;
            UpdatedAt = now;
            UpdatedBy = null;
        }
    }
}
