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

        private IConfigurationSection jwtSettings;

        public AuthService(UserManager<User> userManager, IConfiguration configuration)
        {
            this.userManager = userManager;

            jwtSettings = configuration.GetSection("Jwt");
        }

        public async Task<string> CreateToken()
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var token = GenerateTokenOption(signingCredentials, claims);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private JwtSecurityToken GenerateTokenOption(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var expiration = DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings.GetSection("lifetime").Value));

            var token = new JwtSecurityToken(
                issuer: jwtSettings.GetSection("Issuer").Value,
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

        private SigningCredentials GetSigningCredentials()
        {
            var key = jwtSettings.GetSection("secret").Value;

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
