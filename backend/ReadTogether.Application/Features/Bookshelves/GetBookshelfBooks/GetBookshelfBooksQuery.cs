using MediatR;

namespace ReadTogether.Application.Features.Bookshelves.GetBookshelfBooks
{
    public record GetBookshelfBooksQuery(int BookshelfId, int PageNumber, int PageSize) : IRequest<GetBookshelfBooksDto>;
}
