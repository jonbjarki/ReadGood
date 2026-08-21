using MediatR;

namespace ReadTogether.Application.Features.Bookshelves.RemoveBookFromBookshelf
{
    public record RemoveBookFromBookshelfCommand(int BookshelfId, string BookId, string UserId, CancellationToken CancellationToken) : IRequest<bool>;
}