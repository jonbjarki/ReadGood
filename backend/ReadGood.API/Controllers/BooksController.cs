using Microsoft.AspNetCore.Mvc;
using MediatR;
using ReadGood.Application.Features.Books.SearchBooks;
using ReadGood.Application.Features.Books.GetBookByKey;
using System.ComponentModel.DataAnnotations;
using ReadGood.API.InputModels.Books;

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
    public async Task<IActionResult> GetBookMetadata(GetBookMetadataInputModel inputModel)
    {
        var query = new GetBookByKeyQuery(inputModel.Key);
        var result = await _mediator.Send(query, HttpContext.RequestAborted);
        return Ok(result.Book);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search(SearchBooksInputModel inputModel) {
        var query = new SearchBooksQuery(inputModel.Title, inputModel.Page, inputModel.PageSize);
        var result = await _mediator.Send(query, HttpContext.RequestAborted);
        return Ok(result.Data); 
    }
}
