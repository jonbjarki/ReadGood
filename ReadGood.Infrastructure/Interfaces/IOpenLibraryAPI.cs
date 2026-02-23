using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ReadGood.Domain.Common;
using ReadGood.Domain.DTOs;
using ReadGood.Infrastructure.Responses;

namespace ReadGood.Infrastructure.Interfaces
{
    public interface IOpenLibraryAPI
    {
        Task<PagedResponse<BookSearchItem>?> Search(string title, CancellationToken cancellationToken, int page = 1, int pageSize = 10);
        Task<BookDetailsDto?> GetBookByKey(string key, CancellationToken cancellationToken);
        Task<AuthorDetailsDto?> GetAuthorByKey(string key, CancellationToken cancellationToken);
    }
}