namespace Application.Services
{
    using System.Threading.Tasks;

    using Domain;
    using Domain.Models;
    
    using Services.Interfaces;

    using Microsoft.AspNetCore.Identity;
    
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public AccountService(UserManager<User> userManager,
            SignInManager<User> signInManager) 
        {
                _userManager = userManager;
                _signInManager = signInManager;
        }
        public async Task<IdentityResult> RegisterAsync(User user, string password)
        {

            if (user == null)
            {
                throw new ArgumentNullException("User cannot be null");
            }

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded) 
            {
                result = await _userManager.AddToRoleAsync(user, RoleNames.User);
            }

            return result;
        }
        public async Task<SignInResult> SignInAsync(User user, string password)
        {

            if (user == null) 
            {
                throw new ArgumentNullException("User cannot be null");
            }

            if (string.IsNullOrEmpty(password)) 
            {
                throw new ArgumentNullException("User password cannot be null");
            }

            var result = await _signInManager.PasswordSignInAsync(user, password, false, false);

            return result;
        }
        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}