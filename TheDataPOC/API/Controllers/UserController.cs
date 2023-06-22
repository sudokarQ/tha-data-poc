namespace API.Controllers
{
    using Application.Services.Interfaces;

    using Domain.Models;

    using Infrastructure.Seeders;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = UserRolesSeeder.AdminName)]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        private readonly UserManager<User> userManager;


        public UserController(IUserService userService, UserManager<User> userManager)
        {
            this.userService = userService;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await userService.GetAsync();

                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user is null)
            {
                return NotFound("Incorrect Id");
            }

            var result = await userService.DeleteUser(user);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return NoContent();
        }

        [HttpPut("addrole/{id}")]
        public async Task<IActionResult> AddToRole(string id, [FromQuery] string role)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user is null)
            {
                return NotFound("User not found");
            }

            try
            {
                var result = await userService.AddRoleToUser(user, role);

                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }

                return Ok("Role added");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("removerole/{id}")]
        public async Task<IActionResult> RemoveRole(string id, [FromQuery] string role)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user is null)
            {
                return NotFound("User not found");
            }

            try
            {
                var result = await userService.RemoveRoleFromUser(user, role);

                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }

                return Ok("Role removed");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
