using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReadGood.API.InputModels.Auth;
using ReadGood.Application.Features.Auth.GoogleSignIn;
using ReadGood.Domain.Entities;

namespace ReadGood.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMediator _mediator;

        public AuthController(UserManager<ApplicationUser> userManager, IMediator mediator)
        {
            _userManager = userManager;
            _mediator = mediator;
        }

        /* [HttpPost("register")]
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
        } */

        /* [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn()
        {
            throw new NotImplementedException();
        } */

        [AllowAnonymous]
        [HttpPost("google")]
        public async Task<IActionResult> GoogleSignInOrRegister([FromBody] GoogleSignInInputModel inputModel, CancellationToken cancellationToken)
        {
            var command = new GoogleSignInCommand(inputModel.IdToken);
            var result = await _mediator.Send(command, cancellationToken);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,    
                Secure = true,      
                SameSite = SameSiteMode.Lax, 
                Expires = DateTimeOffset.UtcNow.AddDays(1) // Sets the lifespan
            };

            // Append the cookie to the HTTP response
            Response.Cookies.Append("auth_token", result.JwtToken, cookieOptions);

            return Ok(result);
        }
    }


}