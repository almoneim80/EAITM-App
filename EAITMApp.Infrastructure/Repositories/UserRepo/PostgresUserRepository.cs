using EAITMApp.Application.Interfaces;
using EAITMApp.Domain.Entities;

namespace EAITMApp.Infrastructure.Repositories.UserRepo
{
    public class PostgresUserRepository : IUserRepository
    {
        Task IUserRepository.AddAsync(User user)
        {
            throw new NotImplementedException();
        }

        Task<User?> IUserRepository.GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        Task<User?> IUserRepository.GetByUsernameAsync(string username)
        {
            throw new NotImplementedException();
        }
    }
}
