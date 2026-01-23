using EAITMApp.Application.Persistence.Repositories;
using EAITMApp.Application.Persistence.Specifications;
using EAITMApp.Infrastructure.Persistence.Specifications;
using Microsoft.EntityFrameworkCore;

namespace EAITMApp.Infrastructure.Persistence.Repositories
{
    public sealed class EfReadRepository<TEntity>(ReadDbContext dbContext) : IReadRepository<TEntity> where TEntity : class
    {
        private readonly ReadDbContext _dbContext = dbContext;

        /// <inheritdoc/>
        public async Task<TEntity?> GetBySpecificationAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
        {
            return await ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyList<TEntity>> ListBySpecificationAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
        {
            return await ApplySpecification(specification).ToListAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<int> CountBySpecificationAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
        {
            return await ApplySpecification(specification).CountAsync(cancellationToken);
        }

        /// <inheritdoc/>
        private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> specification)
        {
            return SpecificationEvaluator<TEntity>.GetQuery(_dbContext.Set<TEntity>(), specification);
        }
    }
}
