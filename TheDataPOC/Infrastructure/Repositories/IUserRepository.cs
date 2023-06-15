namespace Infrastructure.Repositories
{
    using Domain.Models;

    public interface IUserRepository : IGenericRepository<User>
    {
        public Task<IList<string>> GetUserRoleAsync(int userId);

        public Task<IList<User>> GetAllUsersAsync();

        public Task<User> FindByNameAsync(string userName);
    }
}