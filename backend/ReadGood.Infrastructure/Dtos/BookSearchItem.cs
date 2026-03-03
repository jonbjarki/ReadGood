namespace ReadGood.Infrastructure.Responses
{
    public class BookSearchItemDto
    {
        public string? CoverImageUrl { get; set; }
        public string Id { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? Author { get; set; }
        public string? FirstPublished { get; set; }
    }

}