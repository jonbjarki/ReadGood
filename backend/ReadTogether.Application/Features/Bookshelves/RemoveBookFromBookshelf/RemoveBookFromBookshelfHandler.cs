using MediatR;
using ReadTogether.Infrastructure.Interfaces;

namespace ReadTogether.Application.Features.Bookshelves.RemoveBookFromBookshelf
{
    public class RemoveBookFromBookshelfHandler : IRequestHandler<RemoveBookFromBookshelfCommand, bool>
    {
        private readonly IBookshelfRepository _bookshelfRepository;

        public RemoveBookFromBookshelfHandler(IBookshelfRepository bookshelfRepository)
        {
            _bookshelfRepository = bookshelfRepository;
        }

        public Task<bool> Handle(RemoveBookFromBookshelfCommand request, CancellationToken cancellationToken)
        {
            return _bookshelfRepository.RemoveBookFromBookshelf(request.BookshelfId, request.BookId, request.UserId, cancellationToken);
        }
    }
}