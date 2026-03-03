using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Xunit;
using Moq;
using Moq.Contrib.HttpClient;
using ReadGood.Infrastructure.Responses;
using Xunit.Abstractions;

namespace ReadGood.Tests.Infrastructure.GoogleBooks
{
    public class GoogleBooksTestsBase

    {
        private static readonly string baseUrl = "https://www.googleapis.com/books/v1/";

        protected static string BaseUrl { get => baseUrl; }

        protected readonly ITestOutputHelper testOutputHelper;
        protected readonly ILoggerFactory loggerFactory;

        public GoogleBooksTestsBase(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
            this.loggerFactory = new LoggerFactory([new XunitLoggerProvider(testOutputHelper)]);
        }

        protected static Mock<HttpMessageHandler> CreateMockHandler()
        {
            var handler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            return handler;
        }

        protected static HttpClient CreateMockHttpClient(Mock<HttpMessageHandler> handler)
        {
            var client = handler.CreateClient();
            client.BaseAddress = new Uri(BaseUrl);
            return client;
        }

        private static readonly Volume mockVolume1 = new()
        {
            Kind = "books#volume",
            Id = "ID123",
            VolumeInfo = new VolumeInfo
            {
                Title = "The Great Gatsby",
                Authors = ["F. Scott Fitzgerald"],
                PublishedDate = "1925",
                ImageLinks = new ImageLinks
                {
                    Thumbnail = "https://example.com/thumbnail.jpg",
                    SmallThumbnail = "https://example.com/thumbnail.jpg"
                }
            }
        };

        private static readonly Volume mockVolume2 = new()
        {
            Kind = "books#volume",
            Id = "ID456",
            VolumeInfo = new VolumeInfo
            {
                Title = "To Kill a Mockingbird",
                Authors = ["Harper Lee"],
                PublishedDate = "1960",
                ImageLinks = new ImageLinks
                {
                    Thumbnail = "https://example.com/thumbnail2.jpg",
                    SmallThumbnail = "https://example.com/thumbnail2.jpg"
                }
            }
        };
        protected static Volume MockVolume1 { get => mockVolume1; }
        protected static Volume MockVolume2 { get => mockVolume2; }
    }

}