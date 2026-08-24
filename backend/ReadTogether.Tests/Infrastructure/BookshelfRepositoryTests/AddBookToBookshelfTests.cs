using ReadTogether.Infrastructure.Exceptions;
using ReadTogether.Infrastructure.Implementations;

namespace ReadTogether.Tests.Infrastructure.BookshelfRepositoryTests
{
    public class AddBookToBookshelfTests : BookshelfRepositoryTestsBase
    {
        [Fact]
        public async Task AddBookToBookshelf_PersistsBook_WhenBookIsNotAlreadyInShelf()
        {
            await using var context = CreateContext();
            var user = CreateUser();
            var bookshelf = CreateBookshelf();
            await SeedAsync(context, user, bookshelf);

            var result = await new BookshelfRepository(context)
                .AddBookToBookshelf(bookshelf.Id, "volume-1", "Test Book", "https://example.test/book.jpg", CancellationToken.None);

            Assert.Equal(bookshelf.Id, result.BookshelfId);
            Assert.Equal("volume-1", result.VolumeId);

            await using var verificationContext = CreateContext();
            var persistedBook = await verificationContext.BookshelfBooks.FindAsync(bookshelf.Id, "volume-1");
            Assert.NotNull(persistedBook);
            Assert.Equal("Test Book", persistedBook.Title);
        }

        [Fact]
        public async Task AddBookToBookshelf_ThrowsConflict_WhenBookAlreadyExistsInShelf()
        {
            await using var context = CreateContext();
            var user = CreateUser();
            var bookshelf = CreateBookshelf();
            await SeedAsync(context, user, bookshelf);
            await SeedAsync(context, CreateBookshelfBook(bookshelf.Id, "volume-1"));

            await Assert.ThrowsAsync<BookshelfBookConflictException>(() => new BookshelfRepository(context)
                .AddBookToBookshelf(bookshelf.Id, "volume-1", "Test Book", "https://example.test/book.jpg", CancellationToken.None));
        }

        [Fact]
        public async Task AddBookToBookshelf_ThrowsConflict_WhenDatabaseRejectsInvalidBookshelfId()
        {
            await using var context = CreateContext();

            await Assert.ThrowsAsync<BookshelfBookConflictException>(() => new BookshelfRepository(context)
                .AddBookToBookshelf(999, "volume-1", "Test Book", "https://example.test/book.jpg", CancellationToken.None));
        }
    }
}