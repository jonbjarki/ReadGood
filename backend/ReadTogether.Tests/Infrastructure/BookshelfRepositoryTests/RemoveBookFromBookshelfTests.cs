using ReadTogether.Infrastructure.Exceptions;
using ReadTogether.Infrastructure.Implementations;

namespace ReadTogether.Tests.Infrastructure.BookshelfRepositoryTests
{
    public class RemoveBookFromBookshelfTests : BookshelfRepositoryTestsBase
    {
        [Fact]
        public async Task RemoveBookFromBookshelf_ReturnsTrueAndDeletesBook_WhenShelfIsOwnedByUser()
        {
            await using var context = CreateContext();
            var user = CreateUser();
            var bookshelf = CreateBookshelf();
            await SeedAsync(context, user, bookshelf);
            var book = CreateBookshelfBook(bookshelf.Id);
            await SeedAsync(context, book);

            var result = await new BookshelfRepository(context)
                .RemoveBookFromBookshelf(bookshelf.Id, book.VolumeId, user.Id, CancellationToken.None);

            Assert.True(result);

            await using var verificationContext = CreateContext();
            Assert.Null(await verificationContext.BookshelfBooks.FindAsync(bookshelf.Id, book.VolumeId));
        }

        [Fact]
        public async Task RemoveBookFromBookshelf_ThrowsAccessDenied_WhenShelfIsNotOwnedByUser()
        {
            await using var context = CreateContext();
            var owner = CreateUser();
            var otherUser = CreateUser(OtherUserId, "John", "john@example.test");
            var bookshelf = CreateBookshelf();
            await SeedAsync(context, owner, otherUser, bookshelf);
            var book = CreateBookshelfBook(bookshelf.Id);
            await SeedAsync(context, book);

            await Assert.ThrowsAsync<AccessDeniedException>(() => new BookshelfRepository(context)
                .RemoveBookFromBookshelf(bookshelf.Id, book.VolumeId, otherUser.Id, CancellationToken.None));

            await using var verificationContext = CreateContext();
            Assert.NotNull(await verificationContext.BookshelfBooks.FindAsync(bookshelf.Id, book.VolumeId));
        }

        [Fact]
        public async Task RemoveBookFromBookshelf_ThrowsNotFound_WhenBookDoesNotExist()
        {
            await using var context = CreateContext();

            await Assert.ThrowsAsync<NotFoundException>(() => new BookshelfRepository(context)
                .RemoveBookFromBookshelf(999, "missing-book", OwnerId, CancellationToken.None));
        }
    }
}