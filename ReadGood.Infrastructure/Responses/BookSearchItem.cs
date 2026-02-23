namespace ReadGood.Infrastructure.Responses
{
    public class BookSearchItem
    {
        public class ImageLinks
        {
            public string? Cover { get; set; }
            public string? Author { get; set; }
        }
        public ImageLinks Images { get; set; } = new ImageLinks();
        public string Key { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? Author { get; set; }
        public int? FirstPublished { get; set; }
    }

}