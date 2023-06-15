namespace Infrastructure.Repositories
{
    using Domain.Models;

    public interface IRoleRepository : IGenericRepository<Role>
    {
        public Task<Role> GetRoleByNameAsync(string roleNormalizedName);

        public Task<IList<User>> GetUsersInRoleAsync(int roleId);
    }
}