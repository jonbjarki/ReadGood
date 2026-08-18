using Moq;
using ReadTogether.Application.Features.Books.GetBookById;
using ReadTogether.Domain.DTOs;
using ReadTogether.Infrastructure.Interfaces;

namespace ReadTogether.Tests.Features.Books
{
    public class GetBookByKeyHandlerTests
    {
        [Fact]
        public async Task ReturnsBook_WhenApiReturnsData()
        {
            // Arrange
            var apiMock = new Mock<IGoogleBooksAPI>();
            var dto = new BookDetailsDto { Title = "Sample" };
            apiMock.Setup(x => x.GetBookById(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dto);

            var handler = new GetBookByIdHandler(apiMock.Object);
            var query = new GetBookByIdQuery("/works/OL123W");

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal("Sample", result.Book?.Title);
        }
    }
}
