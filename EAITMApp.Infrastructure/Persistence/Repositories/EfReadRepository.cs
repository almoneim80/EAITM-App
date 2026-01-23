using EAITMApp.Application.Persistence.Repositories;
using EAITMApp.Application.Persistence.Specifications;
using EAITMApp.Infrastructure.Persistence.Specifications;
using Microsoft.EntityFrameworkCore;

namespace EAITMApp.Infrastructure.Persistence.Repositories
{
    public sealed class EfReadRepository<TEntity>(DbContext dbContext) : IReadRepository<TEntity> where TEntity : class
    {
        private readonly DbContext _dbContext = dbContext;

        public async Task<TEntity?> GetBySpecificationAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
        {
            return await ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<TEntity>> ListBySpecificationAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
        {
            return await ApplySpecification(specification).ToListAsync(cancellationToken);
        }

        public async Task<int> CountBySpecificationAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
        {
            return await ApplySpecification(specification).CountAsync(cancellationToken);
        }

        private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> specification)
        {
            return SpecificationEvaluator<TEntity>.GetQuery(_dbContext.Set<TEntity>(), specification);
        }
    }
}
