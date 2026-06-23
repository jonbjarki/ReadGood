namespace ReadGood.Application.Features.Bookshelves.AddBookToBookshelf
{
    public class AddBookToBookshelfDto
    {
        public required int BookshelfId { get; set; }
        public required string VolumeId { get; set; }
    }
}
