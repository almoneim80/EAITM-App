using EAITMApp.Application.Interfaces;
using EAITMApp.Application.Persistence;
using EAITMApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EAITMApp.Infrastructure.Repositories.TaskRepo
{
    public class TodoTaskReadRepository(IReadDbContext context) : IReadTodoTaskRepository
    {
        private readonly IReadDbContext _context = context;

        public bool ExistsByTitleAsync(string title, CancellationToken cancellationToken)
        {
            return _context.Set<TodoTask>().Any(x => x.Title == title);
        }

        /// <inheritdoc/>
        public IQueryable<TodoTask> GetAll()
        {
            return _context.Set<TodoTask>();
        }

        /// <inheritdoc/>
        public async Task<TodoTask?> GetByIdAsync(Guid id)
        {
            return await _context.Set<TodoTask>().AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id.Equals(id));
        }

        Task<IQueryable<TodoTask>> IReadTodoTaskRepository.GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
