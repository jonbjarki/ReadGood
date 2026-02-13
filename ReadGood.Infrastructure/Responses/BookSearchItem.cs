namespace ReadGood.Application.Features.Books.SearchBooks
{
    public class BookSearchItem
    {
        public string Key { get; set; } = null!;
        public string Title { get; set; } = null!;
        public int FirstPublished { get; set; }
    }
}