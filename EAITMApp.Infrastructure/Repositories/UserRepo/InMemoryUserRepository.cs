using EAITMApp.Application.Interfaces;
using EAITMApp.Domain.Entities;

namespace EAITMApp.Infrastructure.Repositories.UserRepo
{
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly List<User> _users = new();

        /// <inheritdoc/>
        public Task  AddAsync(User user)
        {
            _users.Add(user);
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task<User?> GetByIdAsync(Guid id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            return Task.FromResult(user);
        }

        /// <inheritdoc/>
        public Task<User?> GetByUsernameAsync(string username)
        {
            var user = _users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(user);
        }
    }
}
