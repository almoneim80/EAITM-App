using EAITMApp.Application.Interfaces;
using EAITMApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EAITMApp.Infrastructure.Repositories.UserRepo
{
    public class PostgresUserRepository(IAppDbContext context) : IUserRepository
    {
        private readonly IAppDbContext _context = context;

        public async Task AddAsync(User user)
        {
            await _context.Set<User>().AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Set<User>()
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Set<User>()
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
        }
    }
}
