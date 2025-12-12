using EAITMApp.Domain.Entities;

namespace EAITMApp.Application.Interfaces
{
    public interface IReadTodoTaskRepository
    {
        /// <summary>
        /// get one task by its id (Guid).
        /// </summary>
        /// <param name="id">task id (Guid)</param>
        /// <returns>return a specific task (TodoTask)</returns>
        Task<TodoTask?> GetByIdAsync(Guid id);

        /// <summary>
        /// get all tasks in DB, memory. as list (TodoTask)
        /// </summary>
        /// <returns>return data boject (TodoTask)</returns>
        IQueryable<TodoTask> GetAll();
    }
}
