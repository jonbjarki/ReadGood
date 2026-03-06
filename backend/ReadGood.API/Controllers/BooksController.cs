using Microsoft.AspNetCore.Mvc;
using MediatR;
using ReadGood.Application.Features.Books.SearchBooks;
using ReadGood.API.InputModels.Books;
using ReadGood.Application.Features.Books.GetBookById;

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
    public async Task<IActionResult> GetSingleBook(string id)
    {
        var query = new GetBookByIdQuery(id);
        var result = await _mediator.Send(query, HttpContext.RequestAborted);
        return Ok(result.Book);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search(SearchBooksInputModel inputModel) {
        var query = new SearchBooksQuery(inputModel.Title, inputModel.Page, inputModel.PageSize, inputModel.Author, inputModel.Subject);
        var result = await _mediator.Send(query, HttpContext.RequestAborted);
        return Ok(result.Data); 
    }
}
