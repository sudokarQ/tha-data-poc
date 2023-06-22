namespace API.Controllers
{
    using Application.DTOs;
    using Application.Services.Interfaces;

    using Domain.Models;

    using Infrastructure.Seeders;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> userManager;

        private readonly SignInManager<User> signInManager;

        private readonly IAuthService authManager;

        public AuthController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IAuthService authManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.authManager = authManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(AuthDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new User
            {
                Email = dto.Email,
                UserName = dto.Email,
            };

            var result = await userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
            {
                if (userManager.Users.Count() == 1)
                {
                    await userManager.AddToRoleAsync(user, UserRolesSeeder.AdminName);
                    await userManager.AddToRoleAsync(user, UserRolesSeeder.ScientistName);
                }

                await signInManager.SignInAsync(user, false);

                var roles = await userManager.GetRolesAsync(user);

                var userDto = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserRoles = roles,
                };

                return Ok(userDto);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return BadRequest(ModelState);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await userManager.FindByEmailAsync(dto.Email);

            if (!await authManager.ValidateUser(dto))
            {
                return NotFound("User not found");
            }

            var result = await signInManager.CheckPasswordSignInAsync(user, dto.Password, false);

            if (result.Succeeded)
            {
                var accessToken = await authManager.CreateToken();

                return Ok(accessToken);
            }

            ModelState.AddModelError(string.Empty, "Incorrect login or password");


            return BadRequest(ModelState);
        }
    }
}