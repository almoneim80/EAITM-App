using EAITMApp.Application.Interfaces;
using EAITMApp.Application.Persistence;
using EAITMApp.Domain.Entities;

namespace EAITMApp.Infrastructure.Repositories.TaskRepo
{
    public class TodoTaskWriteRepository(IWriteDbContext context) : IWriteTodoTaskRepository
    {
        private readonly IWriteDbContext _context = context;
        /// <inheritdoc/>
        public async Task<TodoTask> AddAsync(TodoTask task)
        {
            Console.WriteLine("<------------- Add Task To Postgres -------------->");
            await _context.Set<TodoTask>().AddAsync(task);
            await _context.SaveChangesAsync();
            return task;
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(TodoTask task)
        {
            _context.Set<TodoTask>().Update(task);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(Guid id)
        {
            var task = await _context.Set<TodoTask>().FindAsync(id);
            if (task != null)
            {
                _context.Set<TodoTask>().Remove(task);
                await _context.SaveChangesAsync();
            }
        }
    }
}
