using MediatR;

namespace ReadGood.Application.Features.Books.SearchBooks
{
    public record SearchBooksQuery(string Title, int Page, int PageSize) : IRequest<BookSearchDto>;
}