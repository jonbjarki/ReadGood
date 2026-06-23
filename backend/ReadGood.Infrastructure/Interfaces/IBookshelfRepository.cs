using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReadGood.Domain.Entities;

namespace ReadGood.Infrastructure.Interfaces
{
    public interface IBookshelfRepository
    {
        Task<Bookshelf> CreateBookshelf(string name, string userId, CancellationToken cancellationToken, bool isDefaultShelf = false);
        Task<Bookshelf?> GetBookshelfById(int id, CancellationToken cancellationToken);
        Task<List<Bookshelf>> GetBookshelvesByUserId(string userId, CancellationToken cancellationToken);
        Task<BookshelfBook> AddBookToBookshelf(int bookshelfId, string bookId, string title, string thumbnailUrl, CancellationToken cancellationToken);
    }
}