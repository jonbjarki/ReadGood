using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReadGood.API.InputModels.Books;

namespace ReadGood.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserInputModel inputModel)
        {
            return await Task.FromResult(Ok());
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
        {
            string? email = User.FindFirstValue(ClaimTypes.Email);
            if (email is null)
            {
                return Unauthorized();
            }
            return Ok(email);
        }
    }
}