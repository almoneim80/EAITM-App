using EAITMApp.Domain.Entities;

namespace EAITMApp.Application.Interfaces
{
    public interface IReadTodoTaskRepository
    {
        /// <summary>
        /// get one task by its title (string).
        /// </summary>
        /// <param name="id">task title (string)</param>
        /// <returns>return a specific task (TodoTask)</returns>
        Task<TodoTask?> GetByIdAsync(Guid id);

        /// <summary>
        /// get one task by its id (Guid).
        /// </summary>
        /// <param name="id">task id (Guid)</param>
        /// <returns>return a specific task (TodoTask)</returns>
        bool ExistsByTitleAsync(string title, CancellationToken cancellationToken);

        /// <summary>
        /// get all tasks in DB, memory. as list (TodoTask)
        /// </summary>
        /// <returns>return data boject (TodoTask)</returns>
        Task<IQueryable<TodoTask>> GetAll();
    }
}
