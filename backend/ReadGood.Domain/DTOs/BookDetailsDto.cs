namespace ReadGood.Domain.DTOs
{
    public class BookDetailsDto
    {
        public string Id { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int? FirstPublishedYear { get; set; }

        public string? AuthorName { get; set; }
        public string? CoverImageUrl { get; set; }
    }
}