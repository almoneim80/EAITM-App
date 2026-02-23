namespace EAITMApp.Domain.Common
{
    /// <summary>
    /// Base class for all entities with a generic identifier.
    /// Includes common audit fields for tracking creation and origin of changes.
    /// </summary>
    /// <typeparam name="TKey">Type of the unique identifier (e.g., int, Guid).</typeparam>
    public abstract class BaseEntityWithId<TKey> : IHasId<TKey> where TKey : notnull, IEquatable<TKey>
    {
        /// <inheritdoc/>
        public TKey Id { get; protected set; } = default!;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseEntityWithId{TKey}"/> class.default constructor.
        /// </summary>
        protected BaseEntityWithId() { }
    }
}
