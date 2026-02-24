using System.Threading;
using System.Threading.Tasks;
using Moq;
using ReadGood.Application.Features.Books.GetBookByKey;
using ReadGood.Domain.DTOs;
using ReadGood.Infrastructure.Interfaces;
using Xunit;

namespace ReadGood.Tests.Features.Books
{
    public class GetBookByKeyHandlerTests
    {
        [Fact]
        public async Task ReturnsBook_WhenApiReturnsData()
        {
            // Arrange
            var apiMock = new Mock<IOpenLibraryAPI>();
            var dto = new BookDetailsDto { Title = "Sample" };
            apiMock.Setup(x => x.GetBookByKey(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dto);

            var handler = new GetBookByKeyHandler(apiMock.Object);
            var query = new GetBookByKeyQuery("/works/OL123W");

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal("Sample", result.Book?.Title);
        }

        [Fact]
        public async Task ThrowsException_WhenApiReturnsNull()
        {
            // Arrange
            var apiMock = new Mock<IOpenLibraryAPI>();
            apiMock.Setup(x => x.GetBookByKey(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((BookDetailsDto?)null);

            var handler = new GetBookByKeyHandler(apiMock.Object);
            var query = new GetBookByKeyQuery("/works/OL123W");

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(query, CancellationToken.None));
        }
    }
}
