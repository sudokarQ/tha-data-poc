namespace Infrastructure.Stores
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Domain.Models;

    using Infrastructure.UnitOfWork;
    
    using Microsoft.AspNetCore.Identity;

    public class RoleStore : IRoleStore<Role>
    {
        private readonly IUnitOfWork uow;

        public RoleStore(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public void Dispose()
        {
            uow.Dispose();
        }

        public async Task<IdentityResult> CreateAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (role == null)
            {
                throw new ArgumentNullException($"Role cannot be null"); ;
            }

            await uow.RoleRepository.AddAsync(role);
            await uow.SaveAsync();

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (role == null)
            {
                throw new ArgumentNullException($"Role does not exist");
            }

            uow.RoleRepository.Update(role);
            await uow.SaveAsync();

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (role == null)
            {
                throw new ArgumentNullException($"Role cannot be null"); ;
            }

            uow.RoleRepository.Remove(role);
            await uow.SaveAsync();

            return IdentityResult.Success;
        }

        public async Task<Role> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!int.TryParse(roleId, out var id))
            {
                throw new ArgumentNullException("Invalid identificator value");
            }

            var role = await uow.RoleRepository.GetByIdAsync(id);

            return role;
        }

        public async Task<Role> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await uow.RoleRepository.GetRoleByNameAsync(normalizedRoleName);
        }

        public Task<string> GetNormalizedRoleNameAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (role == null)
            {
                throw new ArgumentNullException($"Role does not exist");
            }

            return Task.FromResult(role.NormalizedRoleName);
        }

        public Task<string> GetRoleIdAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (role == null)
            {
                throw new ArgumentNullException($"Role does not exist");
            }

            return Task.FromResult(role.Id.ToString());
        }

        public Task<string> GetRoleNameAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (role == null)
            {
                throw new ArgumentNullException($"Role does not exist");
            }

            return Task.FromResult(role.RoleName);
        }

        public Task SetNormalizedRoleNameAsync(Role role, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (role == null)
            {
                throw new ArgumentNullException($"Role does not exist");
            }

            role.NormalizedRoleName = normalizedName;

            return Task.CompletedTask;
        }

        public Task SetRoleNameAsync(Role role, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (role == null)
            {
                throw new ArgumentNullException($"Role does not exist");
            }

            role.RoleName = roleName;

            return Task.CompletedTask;
        }
    }
}