using ReadGood.Infrastructure.Interfaces;
using System.Net.Http.Json;
using ReadGood.Domain.Common;
using ReadGood.Infrastructure.Responses;
using ReadGood.Domain.DTOs;
using ReadGood.Infrastructure.Exceptions;
using System.Text.Json;
namespace ReadGood.Infrastructure.Implementations
{
    public class OpenLibraryAPI : IOpenLibraryAPI
    {
        private readonly HttpClient httpClient;
        public OpenLibraryAPI(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        private static string? GetCoverUrl(int? cover_i, string size = "M")
        {
            if (cover_i is null) return null;

            return $"https://covers.openlibrary.org/b/id/{cover_i}-{size}.jpg";
        }

        private static string? GetAuthorCoverUrl(string? OLID, string size = "M")
        {
            if (OLID is null) return null;
            return $"https://covers.openlibrary.org/a/olid/{OLID}-{size}.jpg";
        }

        public async Task<PagedResponse<BookSearchItem>?> Search(string title, CancellationToken cancellationToken, int page = 1, int pageSize = 10)
        {
            var query = $"/search.json?title={Uri.EscapeDataString(title)}&fields=title,author_name,author_key,key,first_publish_year,cover_i&lang=eng&limit={pageSize}&offset={(page - 1) * pageSize}";
            try
            {
                var res = await httpClient.GetAsync(query, cancellationToken);

                if (!res.IsSuccessStatusCode)
                {
                    var errorContent = await res.Content.ReadAsStringAsync(cancellationToken);
                    throw new OpenLibraryApiException(
                        $"OpenLibrary API returned {(int)res.StatusCode} for book search",
                        query,
                        (int)res.StatusCode,
                        errorContent
                    );
                }

                var response = await res.Content.ReadFromJsonAsync<OpenLibrarySearchResponse>(cancellationToken);

                if (response?.Docs is null)
                {
                    throw new OpenLibraryApiException(
                        "OpenLibrary API returned empty search results",
                        query
                    );
                }

                var data = response.Docs.Select(doc => new BookSearchItem
                {
                    Key = doc.Key,
                    Title = doc.Title,
                    Author = doc.Author_name.FirstOrDefault(),
                    FirstPublished = doc.First_publish_year,
                    Images = new BookSearchItem.ImageLinks
                    {
                        Cover = GetCoverUrl(doc.Cover_i),
                        Author = GetAuthorCoverUrl(doc.Author_key.FirstOrDefault())
                    }
                }).ToList();

                return new PagedResponse<BookSearchItem>
                {
                    Page = page,
                    PageSize = pageSize,
                    Results = data,
                    Total = response.Num_found
                };
            }
            catch (HttpRequestException ex)
            {
                throw new OpenLibraryApiException(
                    $"Failed to connect to OpenLibrary API: {ex.Message}",
                    query,
                    null,
                    null,
                    ex
                );
            }
            catch (OperationCanceledException ex)
            {
                throw new OpenLibraryApiException(
                    "OpenLibrary API request was cancelled",
                    query,
                    null,
                    null,
                    ex
                );
            }
            catch (JsonException ex)
            {
                throw new OpenLibraryApiException(
                    "Failed to deserialize OpenLibrary API response",
                    query,
                    null,
                    null,
                    ex
                );
            }
        }

        public async Task<BookDetailsDto?> GetBookByKey(string key, CancellationToken cancellationToken)
        {
            var url = key;
            try
            {
                var res = await httpClient.GetAsync(url, cancellationToken);

                if (!res.IsSuccessStatusCode)
                {
                    var errorContent = await res.Content.ReadAsStringAsync(cancellationToken);
                    throw new OpenLibraryApiException(
                        $"OpenLibrary API returned {(int)res.StatusCode} when retrieving book details",
                        url,
                        (int)res.StatusCode,
                        errorContent
                    );
                }

                var bookData = await res.Content.ReadFromJsonAsync<OpenLibraryWorkResponse>(cancellationToken);

                if (bookData is null)
                {
                    throw new OpenLibraryApiException(
                        "OpenLibrary API returned null book data",
                        url
                    );
                }
                string? description = bookData.Description?.ToString();


                AuthorDetailsDto? authorDetails = await GetAuthorDetailsDto(bookData.Authors, cancellationToken);

                return new BookDetailsDto
                {
                    Title = bookData.Title,
                    FirstPublishedYear = bookData.First_publish_date,
                    Description = description,
                    AuthorDetails = authorDetails,
                };
            }
            catch (HttpRequestException ex)
            {
                throw new OpenLibraryApiException(
                    $"Failed to connect to OpenLibrary API: {ex.Message}",
                    url,
                    null,
                    null,
                    ex
                );
            }
            catch (OperationCanceledException ex)
            {
                throw new OpenLibraryApiException(
                    "OpenLibrary API request was cancelled",
                    url,
                    null,
                    null,
                    ex
                );
            }
            catch (JsonException ex)
            {
                throw new OpenLibraryApiException(
                    "Failed to deserialize book data from OpenLibrary API",
                    url,
                    null,
                    null,
                    ex
                );
            }
        }

        // Extracts author information from bookData JSON response and returns the mapped DTO
        private async Task<AuthorDetailsDto?> GetAuthorDetailsDto(AuthorRef[]? authors, CancellationToken cancellationToken)
        {
            var authorKey = ExtractAuthorKeyFromJSON(authors);
            if (authorKey is not null)
            {
                try
                {
                    var author = await GetAuthorByKey(authorKey, cancellationToken);
                    if (author is not null)
                    {
                        return new AuthorDetailsDto
                        {
                            Bio = author.Bio,
                            FullerName = author.FullerName,
                        };

                    }
                }
                catch (OpenLibraryApiException authorEx)
                {
                    // Log but don't fail the entire request if author details fail
                    Console.WriteLine($"Warning: Failed to retrieve author details: {authorEx.Message}");
                }
            }

            return null;
        }

        private string? ExtractAuthorKeyFromJSON(AuthorRef[]? authors)
        {
            if (authors is not null)
            {
                var authorRef = authors.FirstOrDefault();
                if (authorRef is not null && authorRef.Author is not null && authorRef.Author.Key is not null)
                {
                    var authorKey = authorRef.Author.Key;
                    try
                    {
                        return authorKey;
                    }
                    catch (OpenLibraryApiException authorEx)
                    {
                        // Log but don't fail the entire request if author details fail
                        Console.WriteLine($"Warning: Failed to retrieve author details: {authorEx.Message}");
                        return null;
                    }
                }
            }
            return null;
        }

        public async Task<AuthorDetailsDto?> GetAuthorByKey(string key, CancellationToken cancellationToken)
        {
            var url = key;
            try
            {
                var res = await httpClient.GetAsync(url, cancellationToken);

                if (!res.IsSuccessStatusCode)
                {
                    var errorContent = await res.Content.ReadAsStringAsync(cancellationToken);
                    throw new OpenLibraryApiException(
                        $"OpenLibrary API returned {(int)res.StatusCode} when retrieving author details",
                        url,
                        (int)res.StatusCode,
                        errorContent
                    );
                }

                var author = await res.Content.ReadFromJsonAsync<OpenLibraryAuthorResponse>(cancellationToken);

                if (author is null)
                {
                    throw new OpenLibraryApiException(
                        "OpenLibrary API returned null author data",
                        url
                    );
                }

                return new AuthorDetailsDto
                {
                    FullerName = author.FullerName ?? string.Empty,
                    Bio = author.Bio?.Value ?? string.Empty
                };
            }
            catch (HttpRequestException ex)
            {
                throw new OpenLibraryApiException(
                    $"Failed to connect to OpenLibrary API: {ex.Message}",
                    url,
                    null,
                    null,
                    ex
                );
            }
            catch (OperationCanceledException ex)
            {
                throw new OpenLibraryApiException(
                    "OpenLibrary API request was cancelled",
                    url,
                    null,
                    null,
                    ex
                );
            }
            catch (JsonException ex)
            {
                throw new OpenLibraryApiException(
                    "Failed to deserialize author data from OpenLibrary API",
                    url,
                    null,
                    null,
                    ex
                );
            }
        }
    }
}