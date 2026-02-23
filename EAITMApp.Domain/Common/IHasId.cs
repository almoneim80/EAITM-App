namespace EAITMApp.Domain.Common
{
    /// <summary>
    /// Defines an entity that has a unique identifier.
    /// This interface allows generic handling of entities regardless of the ID type.
    /// </summary>
    public interface IHasId<out TKey>
    {
        /// <summary>
        /// Unique identifier of the entity.
        /// </summary>
        TKey Id { get; }
    }
}
