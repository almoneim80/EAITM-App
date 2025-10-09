using EAITMApp.Domain.Entities;

namespace EAITMApp.Application.Interfaces
{
    public interface ITodoTaskRepository
    {
        /// <summary>
        /// add new task.
        /// </summary>
        /// <param name="task">is an data object (TodoTask)</param>
        /// <returns>return new added task</returns>
        Task<TodoTask> AddAsync(TodoTask task);

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
        Task<List<TodoTask>> GetAllAsync();

        /// <summary>
        /// update one task.
        /// </summary>
        /// <param name="task">is the task that we want to update (TodoTask)</param>
        /// <returns>return nothing</returns>
        Task UpdateAsync(TodoTask task);


        /// <summary>
        /// delete one task.
        /// </summary>
        /// <param name="id">is the id of task that we want to delete</param>
        /// <returns>return nothing</returns>
        Task DeleteAsync(Guid id);
    }
}
