namespace Application.Services
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
   
    using Domain.Models;
    
    using DTOs;
    
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    
    using Services.Interfaces;

    public class AuthService : IAuthService
    {
        private readonly UserManager<User> userManager;

        private User user;

        private IConfiguration configuration;

        public AuthService(UserManager<User> userManager, IConfiguration configuration)
        {
            this.userManager = userManager;

            this.configuration = configuration;
        }

        public async Task<string> CreateToken()
        {
            var signingCredentials = GetSigningCredentials(configuration);
            var claims = await GetClaims();
            var token = GenerateTokenOption(signingCredentials, claims, configuration);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private JwtSecurityToken GenerateTokenOption(SigningCredentials signingCredentials, List<Claim> claims, IConfiguration configuration)
        {
            var expiration = DateTime.Now.AddMinutes(Convert.ToDouble(configuration["lifetime"]));

            var token = new JwtSecurityToken(
                issuer: configuration["Issuer"],
                claims: claims,
                expires: expiration,
                signingCredentials: signingCredentials,
                audience: "TheDataPOC.API"
                );

            return token;
        }

        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var roles = await userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        private SigningCredentials GetSigningCredentials(IConfiguration configuration)
        {
            var key = configuration["secret"];
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        public async Task<bool> ValidateUser(AuthDto loginUserDto)
        {
            user = await userManager.FindByNameAsync(loginUserDto.Email);

            return user is not null;
        }
    }
}
