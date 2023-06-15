using Domain.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Application.Services.Interfaces
{
    public interface IUserManagementService
    {
        public Task<IList<string>> GetUserRolesAsync(User user);

        public Task<IEnumerable<User>> GetAllUsersAsync();

        public Task<User> GetUserByUserNameAsync(string userName);

        public Task<User> GetUserByIdAsync(int userId);

        public Task<IdentityResult> UpdateUserAsync(User fromUser, User toUser);

        public Task<IdentityResult> DeleteUserAsync(User user);
    }
}