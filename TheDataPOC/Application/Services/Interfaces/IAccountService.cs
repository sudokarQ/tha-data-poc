namespace Application.Services.Interfaces
{
    using Domain.Models;

    using System.Threading.Tasks;
    
    using Microsoft.AspNetCore.Identity;

    public interface IAccountService
    {
        public Task<IdentityResult> RegisterAsync(User user, string password);

        public Task<SignInResult> SignInAsync(User user, string password);

        public Task SignOutAsync();
    }
}