namespace Infrastructure.Repositories
{
    using Domain.Models;

    using Infrastructure.Database;

    using Microsoft.EntityFrameworkCore;

    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(ApplicationContext context)
            : base (context)
        { 

        }

        public async Task<Role> GetRoleByNameAsync(string normalizedRoleName)
        {
            var result = await context.Set<Role>()
                .SingleOrDefaultAsync(r => r.NormalizedRoleName == normalizedRoleName);

            return result;
        }

        public async Task<IList<User>> GetUsersInRoleAsync(int roleId)
        {
            var users = await context.Set<UserRole>()
                .Where(userRole => userRole.RoleId == roleId)
                .Select(userRole => userRole.User)
                .ToListAsync();

            return users;
        }
    }
}