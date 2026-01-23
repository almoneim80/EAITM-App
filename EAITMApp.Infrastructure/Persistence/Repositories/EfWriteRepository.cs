using EAITMApp.Application.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EAITMApp.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// EF Core implementation of Write Repository.
    /// </summary>
    public sealed class EfWriteRepository<TEntity, TKey>(WriteDbContext context) : IWriteRepository<TEntity, TKey>
    where TEntity : class
    {
        protected readonly WriteDbContext _context = context;
        protected readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

        /// <inheritdoc/>
        public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddRangeAsync(entities, cancellationToken);
        }

        /// <inheritdoc/>
        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        /// <inheritdoc/>
        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            _dbSet.UpdateRange(entities);
        }

        /// <inheritdoc/>
        public void Remove(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        /// <inheritdoc/>
        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
        }
    }
}
