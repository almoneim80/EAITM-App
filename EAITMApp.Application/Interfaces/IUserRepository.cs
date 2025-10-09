using EAITMApp.Domain.Entities;

namespace EAITMApp.Application.Interfaces
{
    public interface IUserRepository
    {
        /// <summary>
        /// Get user by id.
        /// </summary>
        /// <param name="id">this is user id (guid)</param>
        /// <returns>get data for one user (User)</returns>
        Task<User?> GetByIdAsync(Guid id);

        /// <summary>
        /// Get user by username.
        /// </summary>
        /// <param name="username">this is user username (string)</param>
        /// <returns>returns data for one user (User)</returns>
        Task<User?> GetByUsernameAsync(string username);

        /// <summary>
        /// Add new user.
        /// </summary>
        /// <param name="user">this is user object (User)</param>
        /// <returns>returns nothing</returns>
        Task AddAsync(User user);
    }
}
