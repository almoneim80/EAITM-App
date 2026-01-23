using EAITMApp.Application.Persistence.Specifications;
using Microsoft.EntityFrameworkCore;

namespace EAITMApp.Infrastructure.Persistence.Specifications
{
    public static class SpecificationEvaluator<TEntity> where TEntity : class
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> specification)
        {
            var query = inputQuery;

            //  Criteria (WHERE)
            if(specification.Criteria != null)
            {
                query = query.Where(specification.Criteria);
            }

            // Includes
            foreach(var include in specification.Includes)
            {
                query = query.Include(include);
            }

            // Ordering
            if(specification.OrderBy != null)
            {
                query = query.OrderBy(specification.OrderBy);
            }
            else if(specification.OrderByDescending != null)
            {
                query = query.OrderByDescending(specification.OrderByDescending);
            }

            // Paging
            if(specification.IsPagingEnabled)
            {
                query = query.Skip(specification.Skip!.Value)
                    .Take(specification.Take!.Value);
            }

            return query;
        }
    }
}
