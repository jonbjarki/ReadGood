using System.Runtime.InteropServices;
using MediatR;

namespace ReadTogether.Application.Features.Books.SearchBooks
{
    public record SearchBooksQuery(string Title, int Page, int PageSize, string? Author = null, string? Subject = null) : IRequest<BookSearchDto>;
}