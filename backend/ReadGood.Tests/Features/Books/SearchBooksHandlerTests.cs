using Moq;
using ReadGood.Application.Features.Books.SearchBooks;
using ReadGood.Domain.Common;
using ReadGood.Infrastructure.Interfaces;
using ReadGood.Infrastructure.Responses;

namespace ReadGood.Tests.Features.Books
{
    public class SearchBooksHandlerTests
    {
        [Fact]
        public async Task Handle_ReturnsData_WhenApiReturnsResult()
        {
            // Arrange
            var apiMock = new Mock<IGoogleBooksAPI>();
            var response = new PagedResponse<BookSearchItemDto>
            {
                Total = 1,
                Results =
                [
                    new BookSearchItemDto { Id = "/works/OL123W", Title = "Test" }
                ]
            };
            apiMock.Setup(x => x.Search(It.IsAny<string>(), It.IsAny<CancellationToken>(), It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(response);

            var handler = new SearchBooksHandler(apiMock.Object);
            var query = new SearchBooksQuery("test", 1, 10, null);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Data.Total);
        }
    }
}
