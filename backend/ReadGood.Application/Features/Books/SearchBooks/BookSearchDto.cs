using ReadGood.Domain.Common;
using ReadGood.Infrastructure.Responses;

namespace ReadGood.Application.Features.Books.SearchBooks
{
    public class BookSearchDto
    {
        public PagedResponse<BookSearchItemDto> Data { get; set; } = null!;
    }
}