using API.ViewModels;
using Application.Services.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IConfiguration _configuration;

        public RegisterController(IConfiguration configuration, IAccountService accountService) {
            _accountService = accountService;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<RegisterViewModel>> Register(RegisterViewModel registerModel) {
            
            if(!ModelState.IsValid) {
                return BadRequest(registerModel);
            }

            var user = new User {
                UserName = registerModel.Login,
                Email = registerModel.Email
            };

            var result = await _accountService.RegisterAsync(user, registerModel.Password);

            if(result.Succeeded) {
                return Ok(new { Message = "User registered successfully" });
            }

            foreach(var error in result.Errors) {
                ModelState.AddModelError("", error.Description);
            }

            return BadRequest(registerModel);
        }
    }
}