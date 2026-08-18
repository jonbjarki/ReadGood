using ReadTogether.Infrastructure.Responses;

namespace ReadTogether.Application.Features.Bookshelves.GetBookshelf
{
    public class GetBookshelfDto
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string UserId { get; set; }
        public List<BookSearchItemDto> Books { get; set; } = new();
    }
}
