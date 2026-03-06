using System.Runtime.InteropServices;
using ReadGood.Domain.Common;
using ReadGood.Domain.DTOs;
using ReadGood.Infrastructure.Responses;

namespace ReadGood.Infrastructure.Interfaces
{
    public interface IGoogleBooksAPI
    {
        Task<PagedResponse<BookSearchItemDto>> Search(string title, CancellationToken cancellationToken, string? author = null, string? subject = null, int page = 1, int pageSize = 10);
        Task<BookDetailsDto> GetBookById(string id, CancellationToken cancellationToken);
        Task<AuthorDetailsDto?> GetAuthorById(string id, CancellationToken cancellationToken);
    }
}