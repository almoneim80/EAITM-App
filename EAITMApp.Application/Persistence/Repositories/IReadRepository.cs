using EAITMApp.Application.Persistence.Specifications;

namespace EAITMApp.Application.Persistence.Repositories
{
    public interface IReadRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> GetBySpecificationAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<TEntity>> ListBySpecificationAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);
        Task<int> CountBySpecificationAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);
    }
}
