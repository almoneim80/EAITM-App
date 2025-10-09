using EAITMApp.Application.Interfaces;
using EAITMApp.Domain.Entities;

namespace EAITMApp.Infrastructure.Repositories
{
    public class InMemoryTodoTaskRepository : ITodoTaskRepository
    {
        private readonly List<TodoTask> _tasks = new();
        private int _nextId = 1;

        /// <inheritdoc/>
        public Task<TodoTask> AddAsync(TodoTask task)
        {
            task.GetType().GetProperty("Id")?.SetValue(task, _nextId++);
            _tasks.Add(task);
            return Task.FromResult(task);
        }

        /// <inheritdoc/>
        public Task<TodoTask?> GetByIdAsync(Guid id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id.Equals(id));
            return Task.FromResult(task);
        }

        /// <inheritdoc/>
        public Task<List<TodoTask>> GetAllAsync()
        {
            return Task.FromResult(_tasks.ToList());
        }

        /// <inheritdoc/>
        public Task UpdateAsync(TodoTask task)
        {
            var existing = _tasks.FirstOrDefault(t => t.Id == task.Id);
            if (existing != null)
            {
                _tasks.Remove(existing);
                _tasks.Add(task);
            }
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task DeleteAsync(Guid id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id.Equals(id));
            if (task != null)
                _tasks.Remove(task);
            return Task.CompletedTask;
        }
    }
}
