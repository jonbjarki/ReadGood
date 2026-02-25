using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Moq.Contrib.HttpClient;
using ReadGood.Domain.DTOs;
using ReadGood.Infrastructure.Implementations;
using ReadGood.Infrastructure.Responses;

namespace ReadGood.Tests.OpenLibrary
{
    public class GetBookByKeyTests
    {
        [Fact]
        public async Task GetBookByKey_ReturnsBookDetails()
        {
            // Arrange
            var handler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handler.SetupRequest(HttpMethod.Get, "https://openlibrary.org/works/OL45883W")
                .ReturnsJsonResponse(new OpenLibraryWorkResponse
                {
                    Title = "The Great Gatsby",
                });

            var client = handler.CreateClient();
            client.BaseAddress = new Uri("https://openlibrary.org");

            var api = new OpenLibraryAPI(client);
            var key = "/works/OL45883W"; // Example key for "The Great Gatsby"

            // Act
            var result = await api.GetBookByKey(key, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BookDetailsDto>(result);
            Assert.Equal("The Great Gatsby", result.Title);
        }
    
    [Fact]
    public async Task GetBookByKey_ThrowsNotFoundException_WhenBookDoesNotExist()
        {
            // Arrange
            var handler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handler.SetupRequest(HttpMethod.Get, "https://openlibrary.org/works/OL99999W")
                .ReturnsResponse(System.Net.HttpStatusCode.NotFound);

            var client = handler.CreateClient();
            client.BaseAddress = new Uri("https://openlibrary.org");

            var api = new OpenLibraryAPI(client);
            var key = "/works/OL45883W"; // Example key for a non-existent book

            // Act
            await Assert.ThrowsAsync<NotFoundException>(async () => await api.GetBookByKey(key, CancellationToken.None));

        }
    }
}
