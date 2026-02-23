using ReadGood.Domain.Common;
using ReadGood.Infrastructure.Responses;

namespace ReadGood.Application.Features.Books.SearchBooks
{
    public class BookSearchDto
    {
        public PagedResponse<BookSearchItem> Data { get; set; } = null!;
    }
}