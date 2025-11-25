using EAITMApp.Application.Interfaces;
using EAITMApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EAITMApp.Infrastructure.Repositories.TaskRepo
{
    public class TodoTaskRepository(IWriteDbContext context) : ITodoTaskRepository
    {
        private readonly IWriteDbContext _context = context;

        /// <inheritdoc/>
        public async Task<TodoTask> AddAsync(TodoTask task)
        {
            Console.WriteLine("<------------- Add Task To Postgres -------------->");
            await _context.TodoTasks.AddAsync(task);
            await _context.SaveChangesAsync();
            return task;
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(Guid id)
        {
            var task = await _context.TodoTasks.FindAsync(id);
            if (task != null)
            {
                _context.TodoTasks.Remove(task);
                await _context.SaveChangesAsync();
            }
        }

        /// <inheritdoc/>
        public async Task<List<TodoTask>> GetAllAsync()
        {
            return await _context.TodoTasks.AsNoTracking().ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<TodoTask?> GetByIdAsync(Guid id)
        {
            return await _context.TodoTasks.AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id.Equals(id));
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(TodoTask task)
        {
            _context.TodoTasks.Update(task);
            await _context.SaveChangesAsync();
        }
    }
}
