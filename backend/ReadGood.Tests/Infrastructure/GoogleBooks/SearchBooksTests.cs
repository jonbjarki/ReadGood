using Microsoft.Extensions.Logging;
using Moq.Contrib.HttpClient;
using ReadGood.Domain.Common;
using ReadGood.Infrastructure.Exceptions;
using ReadGood.Infrastructure.Implementations;
using ReadGood.Infrastructure.Responses;
using Xunit.Abstractions;

namespace ReadGood.Tests.Infrastructure.GoogleBooks
{
    public class SearchBooksTests(ITestOutputHelper testOutputHelper) : GoogleBooksTestsBase(testOutputHelper)
    {
        [Fact]
        public async Task SearchBooks_ReturnsResults()
        {
            // Arrange
            var handler = CreateMockHandler();
            var query = GoogleBooksAPI.GetSearchQueryUrl("test", 1, 10);
            var fullPath = BaseUrl + query;
            handler.SetupRequest(HttpMethod.Get, fullPath)
                .ReturnsJsonResponse(new GoogleBooksSearchResponse
                {
                    Kind = "books#volumes",
                    TotalItems = 2,
                    Items = [MockVolume1, MockVolume2]
                });

            var client = CreateMockHttpClient(handler);
            var logger = loggerFactory.CreateLogger<GoogleBooksAPI>();
            var api = new GoogleBooksAPI(client, logger);

            // Act
            var result = await api.Search("test", CancellationToken.None, 1, 10);

            // Assert
            Assert.IsType<PagedResponse<BookSearchItemDto>>(result);
            Assert.Equal(2, result.Total);
            Assert.Equal(1, result.Page);
            Assert.Equal(10, result.PageSize);
            Assert.Single(result.Results, r => r.Id == MockVolume1.Id);
            Assert.Single(result.Results, r => r.Id == MockVolume2.Id);
        }

        [Fact]
        public async Task SearchBooks_ReturnsEmptyResults_WhenNoBooksFound()
        {
            // Arrange
            var handler = CreateMockHandler();
            var query = GoogleBooksAPI.GetSearchQueryUrl("test", 1, 10);
            var fullPath = BaseUrl + query;
            handler.SetupRequest(HttpMethod.Get, fullPath)
                .ReturnsJsonResponse(new GoogleBooksSearchResponse
                {
                    Kind = "books#volumes",
                    TotalItems = 0,
                    Items = Array.Empty<Volume>()
                });

            var client = CreateMockHttpClient(handler);
            var logger = loggerFactory.CreateLogger<GoogleBooksAPI>();
            var api = new GoogleBooksAPI(client, logger);

            // Act
            var result = await api.Search("test", CancellationToken.None, 1, 10);

            // Assert
            Assert.IsType<PagedResponse<BookSearchItemDto>>(result);
            Assert.Equal(0, result.Total);
            Assert.Equal(1, result.Page);
            Assert.Equal(10, result.PageSize);
            Assert.Empty(result.Results);
        }

        [Fact]
        public async Task SearchBooks_ThrowsGoogleBooksRateLimitExceededException_WhenRateLimitExceeded()
        {
            // Arrange
            var handler = CreateMockHandler();
            var query = GoogleBooksAPI.GetSearchQueryUrl("test", 1, 10);
            var fullPath = BaseUrl + query;
            handler.SetupRequest(HttpMethod.Get, fullPath)
                .ReturnsResponse(System.Net.HttpStatusCode.TooManyRequests);

            var client = CreateMockHttpClient(handler);
            var api = new GoogleBooksAPI(client, loggerFactory.CreateLogger<GoogleBooksAPI>());

            // Act & Assert
            await Assert.ThrowsAsync<GoogleBooksRateLimitExceededException>(
                async () => await api.Search("test", CancellationToken.None, 1, 10));
        }

        [Theory]
        [InlineData(System.Net.HttpStatusCode.InternalServerError)]
        [InlineData(System.Net.HttpStatusCode.BadRequest)]
        public async Task SearchBooks_ThrowsGoogleBooksApiException_OnNonSuccessStatusCodes(System.Net.HttpStatusCode statusCode)
        {
            // Arrange
            var handler = CreateMockHandler();
            var query = GoogleBooksAPI.GetSearchQueryUrl("test", 1, 10);
            var fullPath = BaseUrl + query;
            handler.SetupRequest(HttpMethod.Get, fullPath)
                .ReturnsResponse(statusCode);

            var client = CreateMockHttpClient(handler);
            var api = new GoogleBooksAPI(client, loggerFactory.CreateLogger<GoogleBooksAPI>());

            // Act & Assert
            await Assert.ThrowsAsync<GoogleBooksApiException>(
                async () => await api.Search("test", CancellationToken.None, 1, 10));
        }

        [Fact]
        public async Task SearchBooks_ThrowsGoogleBooksApiException_WhenResponseContainsNoItems()
        {
            // Arrange
            var handler = CreateMockHandler();
            var query = GoogleBooksAPI.GetSearchQueryUrl("test", 1, 10);
            var fullPath = BaseUrl + query;
            handler.SetupRequest(HttpMethod.Get, fullPath)
                .ReturnsJsonResponse(new GoogleBooksSearchResponse
                {
                    Kind = "books#volumes",
                    TotalItems = 0,
                    Items = null
                });

            var client = CreateMockHttpClient(handler);
            var api = new GoogleBooksAPI(client, loggerFactory.CreateLogger<GoogleBooksAPI>());

            // Act & Assert
            await Assert.ThrowsAsync<ReadGood.Infrastructure.Exceptions.GoogleBooksApiException>(
                async () => await api.Search("test", CancellationToken.None, 1, 10));
        }

        [Fact]
        public void GetSearchQueryUrl_GeneratesCorrectQueryString()
        {
            // simple case
            var url = GoogleBooksAPI.GetSearchQueryUrl("hello world", 3, 5);
            // page 3, pageSize 5 -> startIndex = 10
            Assert.Equal("volumes?q=hello%20world&startIndex=10&maxResults=5", url);

            // verify that special characters are escaped
            var url2 = GoogleBooksAPI.GetSearchQueryUrl("c# books", 1, 1);
            Assert.Equal("volumes?q=c%23%20books&startIndex=0&maxResults=1", url2);
        }

    }
}