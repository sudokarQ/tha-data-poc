namespace Application.Services.Interfaces
{
    using Domain.Models;
    
    using DTOs;
    
    using Microsoft.AspNetCore.Identity;

    public interface IUserService
    {
        Task<List<UserDto>> GetAsync();

        Task<IdentityResult> DeleteUser(User user);

        Task<IdentityResult> AddRoleToUser(User user, string roleName);

        Task<IdentityResult> RemoveRoleFromUser(User user, string roleName);
    }
}
