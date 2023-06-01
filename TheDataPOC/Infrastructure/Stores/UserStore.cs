namespace Infrastructure.Stores
{
    using Domain.Models;
    using Infrastructure.UnitOfWork;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using System.Linq;

    public class UserStore : IUserStore<User>,
        IUserPasswordStore<User>,
        IUserRoleStore<User>
    {
        private readonly IUnitOfWork uow;

        public UserStore(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        #region IUserStore

        public async Task<IdentityResult> CreateAsync(User user,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (user == null)
            {
                throw new ArgumentNullException($"User cannot be null"); ;
            }

            await uow.GetRepository<User>().AddAsync(user);
            await uow.SaveAsync();

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (user == null)
            {
                throw new ArgumentNullException($"User does not exist");
            }

            uow.GetRepository<User>().Remove(user);
            await uow.SaveAsync();

            return IdentityResult.Success;
        }

        public async Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!int.TryParse(userId, out var id))
            {
                throw new ArgumentNullException("Invalid identificator value");
            }

            var user = await uow.GetRepository<User>().GetByIdAsync(id);

            return user;
        }

        public async Task<User> FindByNameAsync(string userName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var repo = uow.GetRepository<User>();

            var user = await repo.GetEntityAsync(user => user.UserName == userName);

            return user;
        }

        public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (user == null)
            {
                throw new ArgumentNullException($"User does not exist");
            }

            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (user == null)
            {
                throw new ArgumentNullException($"User does not exist");
            }

            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (user == null)
            {
                throw new ArgumentNullException($"User does not exist");
            }

            return Task.FromResult(user.UserName);
        }

        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (user == null)
            {
                throw new ArgumentNullException($"User does not exist"); ;
            }

            uow.GetRepository<User>().Update(user);
            await uow.SaveAsync();

            return IdentityResult.Success;
        }

        public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (user == null)
            {
                throw new ArgumentNullException($"User does not exist"); ;
            }

            user.NormalizedUserName = normalizedName;

            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (user == null)
            {
                throw new ArgumentNullException($"User does not exist");
            }

            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException($"Password does not exist");
            }

            user.UserName = userName;
            return Task.CompletedTask;
        }

        #endregion

        #region IUserPasswordStore

        public Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (user == null)
            {
                throw new ArgumentNullException($"User does not exist");
            }

            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (user == null)
            {
                throw new ArgumentNullException($"User does not exist");
            }

            return Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));
        }

        public Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (user == null)
            {
                throw new ArgumentNullException($"User does not exist");
            }

            if (string.IsNullOrEmpty(passwordHash))
            {
                throw new ArgumentNullException($"Password does not exist");
            }

            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        #endregion

        #region IUserRoleStore

        public async Task AddToRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (user == null)
            {
                throw new ArgumentNullException($"User does not exist");
            }

            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentNullException($"Argument roleName is empty");
            }

            var role = await uow.GetRepository<Role>().GetEntityAsync(role => role.RoleName == roleName);

            if(role == null)
            {
                throw new ArgumentNullException($"Role name does not exist");
            }

            var userRole = new UserRole {
                RoleId = role.Id,
                UserId = user.Id
            };

            await uow.GetRepository<UserRole>().AddAsync(userRole);
            await uow.SaveAsync();

            await uow.SaveAsync();
        }

        public async Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (user == null)
            {
                throw new ArgumentNullException($"User does not exist");
            }

            var roles = await uow.GetRepository<User>().GetUserRolesAsync(user.Id);

            return roles;
        }

        public async Task<IList<User>> GetUsersInRoleAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(normalizedRoleName))
            {
                throw new ArgumentNullException($"Role name does not exist");
            }

            var role = await uow.GetRepository<Role>().GetEntityAsync(role => role.NormalizedRoleName == normalizedRoleName);

            if (role == null)
            {
                throw new ArgumentNullException($"Role does not exist");
            }

            var users = await uow.GetRepository<User>().GetUsersInRoleAsync(role.Id);

            return users.ToList();
        }

        public async Task<bool> IsInRoleAsync(User user, string normalizedRoleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (user == null)
            {
                throw new ArgumentNullException($"User does not exist");
            }

            if (string.IsNullOrWhiteSpace(normalizedRoleName))
            {
                throw new ArgumentNullException($"Role name does not exist");
            }

            var role = await uow.GetRepository<Role>().GetEntityAsync(role => role.NormalizedRoleName == normalizedRoleName);

            if (role == null)
            {
                throw new ArgumentNullException($"Role does not exist");
            }

            var userRole = await uow.GetRepository<UserRole>()
                .GetEntityAsync(user => user.Role.Id == role.Id);

            return userRole != null;
        }

        public async Task RemoveFromRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (user == null)
            {
                throw new ArgumentNullException($"User does not exist");
            }

            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentNullException($"Role name does not exist");
            }

            var role = await uow.GetRepository<Role>().GetEntityAsync(role => role.RoleName == roleName);

            if(role == null)
            {
                throw new ArgumentNullException($"Role does not exist");
            }

            await uow.SaveAsync();
        }

        public void Dispose()
        {
            uow.Dispose();
        }

        #endregion
    }
}