using ReadTogether.Domain.Common;
using ReadTogether.Infrastructure.Responses;

namespace ReadTogether.Application.Features.Books.SearchBooks
{
    public class BookSearchDto
    {
        public PagedResponse<BookSearchItemDto> Data { get; set; } = null!;
    }
}