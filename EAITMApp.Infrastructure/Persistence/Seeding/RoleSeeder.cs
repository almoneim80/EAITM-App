using System.Data;

namespace EAITMApp.Infrastructure.Persistence.Seeding
{
    public class RoleSeeder(WriteDbContext context) : IDataSeeder
    {
        public int Order => 1; // run first.

        public async Task SeedAsync(CancellationToken cancellationToken = default)
        {
            var roles = new List<string> { "Admin", "User", "Manager" };

            foreach (var roleName in roles)
            {
                // check if it's not exisit.
                //if (!await context.Roles.AnyAsync(r => r.Name == roleName, cancellationToken))
                //    await context.Roles.AddAsync(new Role { Name = roleName }, cancellationToken);
            }
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
