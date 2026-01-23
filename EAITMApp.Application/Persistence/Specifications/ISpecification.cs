using System.Linq.Expressions;

namespace EAITMApp.Application.Persistence.Specifications
{
    public interface ISpecification<TEntity>
    {
        Expression<Func<TEntity, bool>>? Criteria { get; }
        IReadOnlyList<Expression<Func<TEntity, object>>> Includes { get; }
        Expression<Func<TEntity, object>>? OrderBy { get; }
        Expression<Func<TEntity, object>>? OrderByDescending { get; }
        int? Skip { get; }
        int? Take { get; }
        bool IsPagingEnabled { get; }
    }
}
