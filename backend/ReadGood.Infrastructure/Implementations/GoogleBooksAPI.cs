using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ReadGood.Domain.Common;
using ReadGood.Domain.DTOs;
using ReadGood.Infrastructure.Exceptions;
using ReadGood.Infrastructure.Exceptions.ReadGood.Infrastructure.Exceptions;
using ReadGood.Infrastructure.Interfaces;
using ReadGood.Infrastructure.Responses;


namespace ReadGood.Infrastructure.Implementations
{
    public class GoogleBooksAPI : IGoogleBooksAPI
    {
        private readonly HttpClient httpClient;
        private readonly ILogger<GoogleBooksAPI> _logger;
        public GoogleBooksAPI(HttpClient httpClient, ILogger<GoogleBooksAPI> logger)
        {
            this.httpClient = httpClient;
            _logger = logger;
        }


        public async Task<AuthorDetailsDto?> GetAuthorById(string id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<BookDetailsDto> GetBookById(string id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private string GetSearchQueryUrl(string query, int page, int pageSize)
        {
            var startIndex = (page - 1) * pageSize;
            return $"/volumes?q={Uri.EscapeDataString(query)}&startIndex={startIndex}&maxResults={pageSize}";
        }

        public async Task<PagedResponse<BookSearchItemDto>> Search(string title, CancellationToken cancellationToken, int page = 1, int pageSize = 10)
        {
            var query = GetSearchQueryUrl(title, page, pageSize);
            _logger.LogInformation("Searching for books with url: {Url}", httpClient.BaseAddress + query);
            var res = await httpClient.GetAsync(query, cancellationToken);

            if (!res.IsSuccessStatusCode)
            {
                var errorContent = await res.Content.ReadAsStringAsync(cancellationToken);
                var statusCode = (int)res.StatusCode;
                if (statusCode == 429)
                {
                    throw new GoogleBooksRateLimitExceededException();
                }

                throw new GoogleBooksApiException(
                    $"Google Books API returned {(int)res.StatusCode} for book search",
                    query,
                    (int)res.StatusCode,
                    errorContent
                );
            }

            var response = await res.Content.ReadFromJsonAsync<GoogleBooksSearchResponse>(cancellationToken);

            if (response == null || response.Items == null) // This means the API returned an empty response, which is unexpected
            {
                throw new GoogleBooksApiException(
                    "Google Books API returned an empty response for book search",
                    query,
                    (int)res.StatusCode,
                    null
                );
            }


            var data = response.Items.Select(book => new BookSearchItemDto
            {
                Id = book.Id ?? "",
                Title = book.VolumeInfo.Title,
                Author = book.VolumeInfo.Authors?.FirstOrDefault() ?? "",
                FirstPublished = book.VolumeInfo.PublishedDate,
                CoverImageUrl = book.VolumeInfo.ImageLinks?.Thumbnail ?? "",

            }).ToList();

            return new PagedResponse<BookSearchItemDto>
            {
                Page = page,
                PageSize = pageSize,
                Results = data,
                Total = response.TotalItems
            };
        }
    }
}