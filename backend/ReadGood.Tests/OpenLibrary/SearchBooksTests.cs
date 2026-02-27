using Moq;
using Moq.Contrib.HttpClient;
using ReadGood.Infrastructure.Exceptions;
using ReadGood.Infrastructure.Implementations;
using ReadGood.Infrastructure.Responses;

namespace ReadGood.Tests.OpenLibrary
{
    public class SearchBooksTests : OpenLibraryTestsBase
    {
        [Fact]
        public async Task SearchBooks_ReturnsResults()
        {
            // Arrange
            var url = OpenLibraryAPI.GetSearchQueryUrl("test", 1, 10);
            var handler = CreateMockHandler();
            handler.SetupRequest(HttpMethod.Get, $"https://openlibrary.org{url}")
                .ReturnsJsonResponse(new OpenLibrarySearchResponse {
                    Num_found = 1,
                    Docs =
                    [
                        new Doc { Key = "/works/OL123W", Title = "Test Book" }
                    ]});

            var client = CreateMockHttpClient(handler);
            var api = new OpenLibraryAPI(client);

            // Act
            var result = await api.Search("test", CancellationToken.None, 1, 10);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Total);
            Assert.Single(result.Results);
            Assert.Equal("/works/OL123W", result.Results.First().Id);
            Assert.Equal("Test Book", result.Results.First().Title);
        }

        [Fact]
        public async Task SearchBooks_ReturnsEmptyResults_WhenNoBooksFound()
        {
            // Arrange
            var query = OpenLibraryAPI.GetSearchQueryUrl("test", 1, 10);
            var handler = CreateMockHandler();
            handler.SetupRequest(HttpMethod.Get, BaseUrl+query)
                .ReturnsJsonResponse(new OpenLibrarySearchResponse
                {
                    Num_found = 0,
                    Docs = [],
                });

            var client = CreateMockHttpClient(handler);
            var api = new OpenLibraryAPI(client);

            // Act
            var result = await api.Search("test", CancellationToken.None, 1, 10);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Total);
            Assert.Empty(result.Results);
        }

        [Fact]
        public async Task SearchBooks_ThrowsOpenLibraryRateLimitExceededException_WhenRateLimitExceeded()
        {
            // Arrange
            var query = OpenLibraryAPI.GetSearchQueryUrl("test", 1, 10);
            var handler = CreateMockHandler();
            handler.SetupRequest(HttpMethod.Get, BaseUrl+query)
                .ReturnsResponse(System.Net.HttpStatusCode.TooManyRequests);

            var client = CreateMockHttpClient(handler);
            var api = new OpenLibraryAPI(client);

            // Act
            var exception = await Assert.ThrowsAsync<OpenLibraryRateLimitExceededException>(async () => await api.Search("test", CancellationToken.None, 1, 10));

            // Assert
            Assert.NotNull(exception);
        }

        [Theory]
        [InlineData(System.Net.HttpStatusCode.InternalServerError)]
        [InlineData(System.Net.HttpStatusCode.BadRequest)]
        public async Task SearchBooks_ThrowsOpenLibraryApiException_OnNonSuccessStatusCodes(System.Net.HttpStatusCode statusCode)
        {
            // Arrange
            var query = OpenLibraryAPI.GetSearchQueryUrl("test", 1, 10);
            var handler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handler.SetupRequest(HttpMethod.Get, BaseUrl+query)
                .ReturnsResponse(statusCode);

            var client = CreateMockHttpClient(handler);
            var api = new OpenLibraryAPI(client);

            // Act
            var exception = await Assert.ThrowsAsync<OpenLibraryApiException>(async () => await api.Search("test", CancellationToken.None, 1, 10));

            // Assert
            Assert.NotNull(exception.StatusCode);
            Assert.Equal((int)statusCode, (int)exception.StatusCode);
        }

    }
}