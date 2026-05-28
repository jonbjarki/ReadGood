using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    }
}