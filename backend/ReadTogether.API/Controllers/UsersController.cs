using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReadTogether.API.InputModels.Users;
using ReadTogether.Application.Features.Users.GetUserByUserName;
using ReadTogether.Application.Features.Users.UpdateUser;

namespace ReadTogether.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet("{userName}")]
        /* Returns the profile information of a user by their username */
        public async Task<IActionResult> GetUserProfile(string userName)
        {
            var query = new GetUserByUserNameQuery(userName);
            var result = await _mediator.Send(query);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [Authorize]
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserInputModel inputModel)
        {
            var authenticatedUserId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)
                ?? User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(authenticatedUserId))
            {
                return Unauthorized();
            }

            if (!string.Equals(id, authenticatedUserId, StringComparison.Ordinal))
            {
                return Forbid();
            }

            var command = new UpdateUserCommand(id, inputModel.UserName, inputModel.Email);
            var result = await _mediator.Send(command);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}