using System;
using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Identity;

namespace Application.Services.Interfaces
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IUnitOfWork _uow;
        private readonly UserManager<User> _userManager;


        public UserManagementService(IUnitOfWork uow,
            UserManager<User> userManager)
        {
            _uow = uow;
            _userManager = userManager;
        }
        

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var collection = await _uow.GetRepository<User>().GetRangeAsync();

            return collection;
        }

        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException($"User name cannot be null");
            }

            var user = await _userManager.FindByNameAsync(userName);

            return user;
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            if (userId == 0)
            {
                throw new ArgumentNullException($"User id cannot be empty or null");
            }

            var user = await _uow.GetRepository<User>().GetByIdAsync(userId);

            return user;
        }

        public async Task<IdentityResult> UpdateUserAsync(User fromUser, User toUser)
        {
            if (fromUser == null || toUser == null)
            {
                throw new ArgumentNullException($"User cannot be null");
            }

            fromUser.UserName = toUser.UserName;
            fromUser.Email = toUser.Email;

            var result = await _userManager.UpdateAsync(fromUser);

            return result;
        }

        public async Task<IdentityResult> DeleteUserAsync(User user)
        {
            if(user == null)
            {
                throw new ArgumentNullException($"Invalid user");
            }

            var result = await _userManager.DeleteAsync(user);

            return result;
        }
    }
}