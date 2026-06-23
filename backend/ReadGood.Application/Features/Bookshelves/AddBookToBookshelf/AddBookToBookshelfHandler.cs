using MediatR;
using ReadGood.Infrastructure.Exceptions;
using ReadGood.Infrastructure.Interfaces;

namespace ReadGood.Application.Features.Bookshelves.AddBookToBookshelf
{
    public class AddBookToBookshelfHandler : IRequestHandler<AddBookToBookshelfCommand, AddBookToBookshelfDto>
    {
        private readonly IBookshelfRepository _bookshelfRepository;

        public AddBookToBookshelfHandler(IBookshelfRepository bookshelfRepository)
        {
            _bookshelfRepository = bookshelfRepository;
        }

        public async Task<AddBookToBookshelfDto> Handle(AddBookToBookshelfCommand request, CancellationToken cancellationToken)
        {
            var bookshelf = await _bookshelfRepository.GetBookshelfById(request.BookshelfId, cancellationToken);
            if (bookshelf is null || bookshelf.UserId != request.UserId)
            {
                throw new NotFoundException("Bookshelf", request.BookshelfId.ToString());
            }

            var bookshelfBook = await _bookshelfRepository.AddBookToBookshelf(request.BookshelfId, request.BookId, request.Title, request.ThumbnailUrl, cancellationToken);
            return new AddBookToBookshelfDto
            {
                BookshelfId = bookshelfBook.BookshelfId,
                VolumeId = bookshelfBook.VolumeId
            };
        }
    }
}
