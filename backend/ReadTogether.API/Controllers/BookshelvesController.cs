using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReadTogether.API.InputModels.Bookshelf;
using ReadTogether.API.InputModels.Bookshelves;
using ReadTogether.Application.Features.Bookshelves.AddBookToBookshelf;
using ReadTogether.Application.Features.Bookshelves.CreateBookshelf;
using ReadTogether.Application.Features.Bookshelves.GetBookshelf;
using ReadTogether.Application.Features.Bookshelves.GetBookshelvesByUser;

namespace ReadTogether.API.Controllers
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

        [HttpGet("user/{username}")]
        public async Task<IActionResult> GetBookshelvesByUser(string username)
        {
            var query = new GetBookshelvesByUserQuery(username);
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

        [HttpPost("{bookshelfId}/books/{bookId}")]
        public async Task<IActionResult> AddBookToBookshelf(int bookshelfId, string bookId, [FromBody] AddBookToBookshelfInputModel inputModel)
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (id == null)
            {
                return Unauthorized();
            }

            var command = new AddBookToBookshelfCommand(bookshelfId, bookId, inputModel.Title, inputModel.ThumbnailUrl, id);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}