namespace ReadGood.Application.Features.Bookshelves.GetBookshelf
{
    public class GetBookshelfDto
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string UserId { get; set; }
    }
}
