using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReadTogether.Domain.Common;
using ReadTogether.Domain.DTOs;
using ReadTogether.Domain.Entities;

namespace ReadTogether.Infrastructure.Interfaces
{
    public interface IBookshelfRepository
    {
        Task<Bookshelf> CreateBookshelf(string name, string userId, CancellationToken cancellationToken, bool isDefaultShelf = false);
        Task<Bookshelf?> GetBookshelfById(int id, CancellationToken cancellationToken);
        Task<PagedResponse<BookshelfBookDto>> GetBookshelfBooks(int bookshelfId, int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<List<Bookshelf>> GetBookshelvesByUserId(string userId, CancellationToken cancellationToken);
        Task<BookshelfBook> AddBookToBookshelf(int bookshelfId, string bookId, string title, string thumbnailUrl, CancellationToken cancellationToken);
        Task<bool> RemoveBookFromBookshelf(int bookshelfId, string bookId, string userId, CancellationToken cancellationToken);
        Task<bool> DeleteBookshelf(int bookshelfId, string userId, CancellationToken cancellationToken);
    }
}