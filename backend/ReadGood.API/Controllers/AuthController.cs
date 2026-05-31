using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReadGood.API.InputModels.Auth;
using ReadGood.Domain.Entities;

namespace ReadGood.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly UserManager<ApplicationUser> _userManager;

        public AuthController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterInputModel inputModel)
        {
            var user = new ApplicationUser
            {
                Email = inputModel.Email,
                UserName = inputModel.UserName,
            };

            var res = await _userManager.CreateAsync(user, inputModel.Password);

            if (res.Succeeded)
            {
                return Ok("User created successfully");
            }
            return BadRequest(res.Errors);
        }
        public async Task<IActionResult> SignIn()
        {
            throw new NotImplementedException();
        }
    }

}