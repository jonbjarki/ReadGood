using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReadGood.API.InputModels.Bookshelf;
using ReadGood.Application.Features.Bookshelves.CreateBookshelf;
using ReadGood.Application.Features.Bookshelves.GetBookshelf;

namespace ReadGood.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookshelvesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookshelvesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookshelf(int id)
        {

            var query = new GetBookshelfQuery(id);
            var result = await _mediator.Send(query, HttpContext.RequestAborted);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBookshelf([FromBody] CreateBookshelfInputModel inputModel)
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (id == null)
            {
                return Unauthorized();
            }

            var command = new CreateBookshelfCommand(inputModel.Name, id);
            var result = await _mediator.Send(command);
            return Created($"/api/bookshelves/{result.Id}", result);
        }
    }
}