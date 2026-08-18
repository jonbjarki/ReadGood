using MediatR;

namespace ReadTogether.Application.Features.Bookshelves.AddBookToBookshelf
{
    public record AddBookToBookshelfCommand(int BookshelfId, string BookId, string Title, string ThumbnailUrl, string UserId) : IRequest<AddBookToBookshelfDto>;
}