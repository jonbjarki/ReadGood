namespace ReadGood.Domain.DTOs
{
    public class BookDetailsDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? FirstPublishedYear { get; set; }

        public string? AuthorName { get; set; }

    }
}