namespace ReadGood.Infrastructure.Responses
{
    public class BookMetadataResponse
    {
        public string Title { get; set; } = string.Empty;
        public DateOnly FirstPublished { get; set; }
    }
}
