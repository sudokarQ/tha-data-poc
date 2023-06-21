namespace Application.Services
{
    using Domain.Models;
    
    using DTOs;
    
    using Infrastructure.UnitOfWork;
    
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using Services.Interfaces;

    public class UserService : IUserService
    {
        private readonly UserManager<User> userManager;

        private readonly IUnitOfWork unitOfWork;

        private readonly RoleManager<IdentityRole> roleManager;

        public UserService(
            UserManager<User> userManager,
            IUnitOfWork unitOfWork,
            RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.unitOfWork = unitOfWork;
            this.roleManager = roleManager;
        }

        public async Task<List<UserDto>> GetAsync()
        {
            var usersList = unitOfWork.GetRepository<User>()
                .Get()
                .ToList();

            List<UserDto> userModels = new List<UserDto>();

            foreach (var user in usersList)
            {
                var roles = await userManager.GetRolesAsync(user);

                userModels.Add(new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserRoles = roles,
                });
            }

            return userModels;
        }

        public async Task<IdentityResult> DeleteUser(User user)
        {
            var result = await userManager.DeleteAsync(user);

            return result;
        }

        public async Task<IdentityResult> AddRoleToUser(User user, string role)
        {
            var roles = await roleManager.Roles.ToListAsync();

            var roleNames = roles.Select(role => role.NormalizedName).ToList();

            if (roleNames.Contains(role.ToUpper()))
            {
                return await userManager.AddToRoleAsync(user, role);
            }

            throw new ArgumentException($"Role {role} does not exist");
        }

        public async Task<IdentityResult> RemoveRoleFromUser(User user, string roleName)
        {
            var result = await userManager.RemoveFromRoleAsync(user, roleName);

            return result;
        }
    }
}
