using Microsoft.EntityFrameworkCore;
namespace EAITMApp.Infrastructure.Persistence
{
    public interface IEFCoreRelationalProvider
    {
        void ConfigureDbContext(DbContextOptionsBuilder options, string connectionString);
    }
}
