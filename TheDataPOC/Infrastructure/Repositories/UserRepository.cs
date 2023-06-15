namespace Infrastructure.Repositories
{
    using Domain.Models;

    using Infrastructure.Database;

    using Microsoft.EntityFrameworkCore;
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationContext context) 
            : base(context)
        {

        }

        public async Task<User> FindByNameAsync(string userName)
        {
            var user = await context.Set<User>()
                .SingleOrDefaultAsync(u => u.UserName == userName);

            return user;
        }

        public async Task<IList<User>> GetAllUsersAsync()
        {
            var users = await context.Set<User>()
                .Include(user => user.UserRoles)
                .ThenInclude(ur => ur.Role)
                .AsNoTracking()
                .ToListAsync();

            return users;
        }

        public async Task<IList<string>> GetUserRoleAsync(int userId)
        {
            var roleNameCollection = await context
                .Set<UserRole>()
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.Role.RoleName)
                .ToListAsync();
            
            return roleNameCollection;
        }
    }
}