using Microsoft.AspNetCore.Mvc;
using ReadGood.Application.Features.Books.GetBookById;
using MediatR;
using ReadGood.Application.Features.Books.SearchBooks;

namespace ReadGood.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IMediator _mediator;

    public BooksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBook(int id)
    {
        var query = new GetBookByIdQuery(id);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string title) {
        var query = new SearchBooksQuery(title);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
