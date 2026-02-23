using Microsoft.AspNetCore.Mvc;
using MediatR;
using ReadGood.Application.Features.Books.SearchBooks;
using ReadGood.Application.Features.Books.GetBookByKey;
using System.ComponentModel.DataAnnotations;

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

    [HttpGet("get")]
    public async Task<IActionResult> GetBookMetadata([FromQuery][Required] string key)
    {
        var query = new GetBookByKeyQuery(key);
        var result = await _mediator.Send(query, HttpContext.RequestAborted);
        return Ok(result.Book);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery][Required] string title,[FromQuery] int page = 1,[FromQuery] int pageSize = 10) {
        var query = new SearchBooksQuery(title, page, pageSize);
        var result = await _mediator.Send(query, HttpContext.RequestAborted);
        return Ok(result.Data); 
    }
}
