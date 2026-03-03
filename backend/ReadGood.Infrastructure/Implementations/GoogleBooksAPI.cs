using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using ReadGood.Domain.Common;
using ReadGood.Domain.DTOs;
using ReadGood.Infrastructure.Exceptions;
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
            var url = $"volumes/{id}";
            _logger.LogInformation("Fetching book details with URL: {Url}", httpClient.BaseAddress + url);

            try
            {
                var res = await httpClient.GetAsync(url, cancellationToken);

                if (!res.IsSuccessStatusCode)
                {
                    var errorContent = await res.Content.ReadAsStringAsync(cancellationToken);
                    var statusCode = (int)res.StatusCode;

                    if (statusCode == 429)
                    {
                        _logger.LogWarning("Rate limit exceeded when fetching book with ID: {BookId}", id);
                        throw new GoogleBooksRateLimitExceededException();
                    }

                    if (statusCode == 404)
                    {
                        _logger.LogWarning("Book not found with ID: {BookId}", id);
                        throw new NotFoundException("Book", id);
                    }

                    _logger.LogError("Google Books API returned error {StatusCode} for book ID {BookId}", statusCode, id);
                    throw new GoogleBooksApiException(
                        $"Google Books API returned {statusCode} when fetching book details",
                        url,
                        statusCode,
                        errorContent
                    );
                }

                var response = await res.Content.ReadFromJsonAsync<Volume>(cancellationToken);

                if (response == null || response.VolumeInfo == null)
                {
                    _logger.LogError("Google Books API returned empty response for book ID: {BookId}", id);
                    throw new GoogleBooksApiException(
                        "Google Books API returned an empty response for book details",
                        url,
                        (int)res.StatusCode,
                        null
                    );
                }

                var volumeInfo = response.VolumeInfo;

                // Extract the year from PublishedDate (format is typically either "YYYY" or "YYYY-MM-DD")
                int? publishedYear = null;
                if (!string.IsNullOrEmpty(volumeInfo.PublishedDate))
                {
                    if (int.TryParse(volumeInfo.PublishedDate.AsSpan(0, 4), out var year))
                    {
                        publishedYear = year;
                    }
                }

                var bookDetails = new BookDetailsDto
                {
                    Title = volumeInfo.Title,
                    Description = volumeInfo.Description,
                    FirstPublishedYear = publishedYear,
                    AuthorName = volumeInfo.Authors?.FirstOrDefault() // Just take the first author for simplicity
                };

                _logger.LogInformation("Successfully fetched book details for ID: {BookId}", id);
                return bookDetails;
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (GoogleBooksRateLimitExceededException)
            {
                throw;
            }
            catch (GoogleBooksApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error when fetching book details for ID: {BookId}", id);
                throw new GoogleBooksApiException(
                    $"Unexpected error when fetching book details for ID '{id}'",
                    url,
                    null,
                    ex.Message,
                    ex
                );
            }
        }

        public static string GetSearchQueryUrl(string query, int page, int pageSize)
        {
            var startIndex = (page - 1) * pageSize;
            var escapedString = Uri.EscapeDataString(query);
            return $"volumes?q={escapedString}&startIndex={startIndex}&maxResults={pageSize}";
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

            if (response is null || response.Items is null) // This means the API returned an empty response, which is unexpected
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
                Id = book.Id ?? throw new GoogleBooksApiException("Google Books API returned a book item with missing ID", query, (int)res.StatusCode, null),
                Title = book.VolumeInfo?.Title ?? throw new GoogleBooksApiException("Google Books API returned a book item with missing title", query, (int)res.StatusCode, null),
                Author = book.VolumeInfo?.Authors?.FirstOrDefault() ?? "",
                FirstPublished = book.VolumeInfo?.PublishedDate,
                CoverImageUrl = book.VolumeInfo?.ImageLinks?.Thumbnail ?? "",

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