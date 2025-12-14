using EAITMApp.Domain.Entities;

namespace EAITMApp.Application.Interfaces
{
    public interface IWriteTodoTaskRepository
    {
        /// <summary>
        /// add new task.
        /// </summary>
        /// <param name="task">is an data object (TodoTask)</param>
        /// <returns>return new added task</returns>
        Task<TodoTask> AddAsync(TodoTask task);

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
