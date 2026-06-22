namespace ReadGood.Application.Features.Bookshelves.GetBookshelvesByUser
{
    public class GetBookshelvesByUserDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsDefaultShelf { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
