namespace API.Controllers
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    
    using API.ViewModels;

    using Application.Services.Interfaces;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.IdentityModel.Tokens;

    [ApiController]
    [Route("api/[controller]")]
    public class LogInController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IUserManagementService _userManagementService;
        private readonly IConfiguration _configuration;

        public LogInController(
            IAccountService accountService,
            IUserManagementService userManagementService,
            IConfiguration configuration) 
        {
            _accountService = accountService;
            _userManagementService = userManagementService;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LogInViewModel logInModel) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(logInModel);
            }

            try
            {
                var user = await _userManagementService.GetUserByUserNameAsync(logInModel.UserName) ;
                var roles = await _userManagementService.GetUserRolesAsync(user);
                var token = GenerateJwtToken(logInModel.UserName, roles);

                ModelState.AddModelError("", "Invalid password");

                return Ok(new { Token = token });
            }
            catch(ArgumentNullException ex)
            {
                ModelState.AddModelError("", "Invalid login attempt");

                return BadRequest(logInModel);
            }
        }

        private string GenerateJwtToken(string username, IList<string> roles)
        {
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:SecretKey"]);

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, roles[0])
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                     SecurityAlgorithms.HmacSha256Signature),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}