using Microsoft.Extensions.Logging;
using Moq;
using Moq.Contrib.HttpClient;
using ReadGood.Domain.DTOs;
using ReadGood.Infrastructure.Exceptions;
using ReadGood.Infrastructure.Implementations;
using ReadGood.Infrastructure.Responses;
using Xunit.Abstractions;

namespace ReadGood.Tests.Infrastructure.GoogleBooks
{
    public class GetBookByIdTests(ITestOutputHelper testOutputHelper) : GoogleBooksTestsBase(testOutputHelper)
    {
        [Fact]
        public async Task GetBookById_ReturnsBookDetails()
        {
            // Arrange
            var handler = CreateMockHandler();
            var id = MockVolume1.Id;
            var query = $"volumes/{id}";
            var fullPath = BaseUrl + query;
            handler.SetupRequest(HttpMethod.Get, fullPath)
                .ReturnsJsonResponse(MockVolume1);

            var client = CreateMockHttpClient(handler);
            var logger = loggerFactory.CreateLogger<GoogleBooksAPI>();

            var api = new GoogleBooksAPI(client, logger);

            // Act
            var result = await api.GetBookById(id, CancellationToken.None);

            // Assert
            Assert.NotNull(result.Title);
            Assert.IsType<BookDetailsDto>(result);
            Assert.Equal(MockVolume1.VolumeInfo!.Title, result.Title);
        }

        [Fact]
        public async Task GetBookById_ThrowsNotFoundException_WhenBookDoesNotExist()
        {
            // Arrange
            var handler = CreateMockHandler();
            var query = "volumes/xxxx"; // Example ID for a non-existent book
            var fullPath = BaseUrl + query;
            handler.SetupRequest(HttpMethod.Get, fullPath)
                .ReturnsResponse(System.Net.HttpStatusCode.NotFound);

            var client = CreateMockHttpClient(handler);

            var api = new GoogleBooksAPI(client, loggerFactory.CreateLogger<GoogleBooksAPI>());
            var key = "xxxx"; // Example key for a non-existent book

            // Act
            var exception = async () => await api.GetBookById(key, CancellationToken.None);

            await Assert.ThrowsAsync<NotFoundException>(exception);
        }

        [Fact]
        public async Task GetBookById_ThrowsGoogleBooksRateLimitExceededException_WhenRateLimitExceeded()
        {
            // Arrange
            var handler = CreateMockHandler();
            var query = $"volumes/{MockVolume1.Id}";
            var fullPath = BaseUrl + query;
            handler.SetupRequest(HttpMethod.Get, fullPath)
                .ReturnsResponse(System.Net.HttpStatusCode.TooManyRequests);

            var client = CreateMockHttpClient(handler);
            var api = new GoogleBooksAPI(client, loggerFactory.CreateLogger<GoogleBooksAPI>());

            // Act and Assert
            await Assert.ThrowsAsync<GoogleBooksRateLimitExceededException>(
                async () => await api.GetBookById(MockVolume1.Id, CancellationToken.None));
        }


        [Fact]
        public async Task GetBookById_ThrowsGoogleBooksApiException_WhenResponseIsNull()
        {
            // Arrange
            var handler = CreateMockHandler();
            var query = $"volumes/{MockVolume1.Id}";
            var fullPath = BaseUrl + query;
            handler.SetupRequest(HttpMethod.Get, fullPath)
                .ReturnsJsonResponse<Volume>(null!); // Simulate null response from API

            var client = CreateMockHttpClient(handler);
            var api = new GoogleBooksAPI(client, loggerFactory.CreateLogger<GoogleBooksAPI>());

            // Act and Assert
            await Assert.ThrowsAsync<GoogleBooksApiException>(
                async () => await api.GetBookById(MockVolume1.Id, CancellationToken.None));
        }

        [Fact]
        public async Task GetBookById_ThrowsGoogleBooksApiException_WhenVolumeInfoIsNull()
        {
            // Arrange
            var handler = CreateMockHandler();
            var query = $"volumes/{MockVolume1.Id}";
            var fullPath = BaseUrl + query;
            var invalidVolume = new Volume
            {
                Kind = "books#volume",
                Id = MockVolume1.Id,
                VolumeInfo = null
            };
            handler.SetupRequest(HttpMethod.Get, fullPath)
                .ReturnsJsonResponse(invalidVolume);

            var client = CreateMockHttpClient(handler);
            var api = new GoogleBooksAPI(client, loggerFactory.CreateLogger<GoogleBooksAPI>());

            // Act and Assert
            await Assert.ThrowsAsync<GoogleBooksApiException>(
                async () => await api.GetBookById(MockVolume1.Id, CancellationToken.None));
        }

        [Fact]
        public async Task GetBookById_HandlesNullAuthors_Gracefully()
        {
            // Arrange
            var handler = CreateMockHandler();
            var query = $"volumes/{MockVolume1.Id}";
            var fullPath = BaseUrl + query;
            var volumeWithoutAuthors = new Volume
            {
                Kind = "books#volume",
                Id = MockVolume1.Id,
                VolumeInfo = new VolumeInfo
                {
                    Title = "Book Without Authors",
                    Authors = null,
                    PublishedDate = "2024",
                    ImageLinks = new ImageLinks
                    {
                        Thumbnail = "https://example.com/thumbnail.jpg",
                        SmallThumbnail = "https://example.com/thumbnail.jpg"
                    }
                }
            };
            handler.SetupRequest(HttpMethod.Get, fullPath)
                .ReturnsJsonResponse(volumeWithoutAuthors);

            var client = CreateMockHttpClient(handler);
            var logger = loggerFactory.CreateLogger<GoogleBooksAPI>();
            var api = new GoogleBooksAPI(client, logger);

            // Act
            var result = await api.GetBookById(MockVolume1.Id, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Book Without Authors", result.Title);
            Assert.Null(result.AuthorName);
        }

        [Fact]
        public async Task GetBookById_ParsesPublishedYear_FromYearOnlyDate()
        {
            // Arrange
            var handler = CreateMockHandler();
            var query = $"volumes/{MockVolume1.Id}";
            var fullPath = BaseUrl + query;
            var volumeWithYearDate = new Volume
            {
                Kind = "books#volume",
                Id = MockVolume1.Id,
                VolumeInfo = new VolumeInfo
                {
                    Title = "Test Book",
                    Authors = ["Test Author"],
                    PublishedDate = "2023",
                    ImageLinks = new ImageLinks
                    {
                        Thumbnail = "https://example.com/thumbnail.jpg",
                        SmallThumbnail = "https://example.com/thumbnail.jpg"
                    }
                }
            };
            handler.SetupRequest(HttpMethod.Get, fullPath)
                .ReturnsJsonResponse(volumeWithYearDate);

            var client = CreateMockHttpClient(handler);
            var logger = loggerFactory.CreateLogger<GoogleBooksAPI>();
            var api = new GoogleBooksAPI(client, logger);

            // Act
            var result = await api.GetBookById(MockVolume1.Id, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2023, result.FirstPublishedYear);
        }

        [Fact]
        public async Task GetBookById_ParsesPublishedYear_FromFullDate()
        {
            // Arrange
            var handler = CreateMockHandler();
            var query = $"volumes/{MockVolume1.Id}";
            var fullPath = BaseUrl + query;
            var volumeWithFullDate = new Volume
            {
                Kind = "books#volume",
                Id = MockVolume1.Id,
                VolumeInfo = new VolumeInfo
                {
                    Title = "Test Book",
                    Authors = ["Test Author"],
                    PublishedDate = "2023-06-15",
                    ImageLinks = new ImageLinks
                    {
                        Thumbnail = "https://example.com/thumbnail.jpg",
                        SmallThumbnail = "https://example.com/thumbnail.jpg"
                    }
                }
            };
            handler.SetupRequest(HttpMethod.Get, fullPath)
                .ReturnsJsonResponse(volumeWithFullDate);

            var client = CreateMockHttpClient(handler);
            var logger = loggerFactory.CreateLogger<GoogleBooksAPI>();
            var api = new GoogleBooksAPI(client, logger);

            // Act
            var result = await api.GetBookById(MockVolume1.Id, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2023, result.FirstPublishedYear);
        }

        [Fact]
        public async Task GetBookById_HandlesNullPublishedDate_Gracefully()
        {
            // Arrange
            var handler = CreateMockHandler();
            var query = $"volumes/{MockVolume1.Id}";
            var fullPath = BaseUrl + query;
            var volumeWithoutDate = new Volume
            {
                Kind = "books#volume",
                Id = MockVolume1.Id,
                VolumeInfo = new VolumeInfo
                {
                    Title = "Book Without Date",
                    Authors = ["Test Author"],
                    PublishedDate = null,
                    ImageLinks = new ImageLinks
                    {
                        Thumbnail = "https://example.com/thumbnail.jpg",
                        SmallThumbnail = "https://example.com/thumbnail.jpg"
                    }
                }
            };
            handler.SetupRequest(HttpMethod.Get, fullPath)
                .ReturnsJsonResponse(volumeWithoutDate);

            var client = CreateMockHttpClient(handler);
            var logger = loggerFactory.CreateLogger<GoogleBooksAPI>();
            var api = new GoogleBooksAPI(client, logger);

            // Act
            var result = await api.GetBookById(MockVolume1.Id, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.FirstPublishedYear);
        }

        [Theory]
        [InlineData(System.Net.HttpStatusCode.InternalServerError)]
        [InlineData(System.Net.HttpStatusCode.BadRequest)]
        [InlineData(System.Net.HttpStatusCode.Unauthorized)]
        public async Task GetBookById_ThrowsGoogleBooksApiException_OnNonSuccessStatusCodes(System.Net.HttpStatusCode statusCode)
        {
            // Arrange
            var handler = CreateMockHandler();
            var query = $"volumes/{MockVolume1.Id}";
            var fullPath = BaseUrl + query;
            handler.SetupRequest(HttpMethod.Get, fullPath)
                .ReturnsResponse(statusCode);

            var client = CreateMockHttpClient(handler);
            var api = new GoogleBooksAPI(client, loggerFactory.CreateLogger<GoogleBooksAPI>());

            // Act and Assert
            await Assert.ThrowsAsync<GoogleBooksApiException>(
                async () => await api.GetBookById(MockVolume1.Id, CancellationToken.None));
        }
    }
}
