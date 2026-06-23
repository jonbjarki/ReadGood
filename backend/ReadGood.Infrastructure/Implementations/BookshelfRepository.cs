using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReadGood.Domain.Contexts;
using ReadGood.Domain.Entities;
using ReadGood.Infrastructure.Exceptions;
using ReadGood.Infrastructure.Interfaces;

namespace ReadGood.Infrastructure.Implementations
{
    public class BookshelfRepository : IBookshelfRepository
    {
        private readonly BooksDbContext _context;

        public BookshelfRepository(BooksDbContext context)
        {
            _context = context;
        }

        public async Task<Bookshelf> CreateBookshelf(string name, string userId, CancellationToken cancellationToken, bool isDefaultShelf = false)
        {
            var newBookshelf = new Bookshelf
            {
                Name = name,
                UserId = userId,
                IsDefaultShelf = isDefaultShelf
            };

            _context.Bookshelves.Add(newBookshelf);
            await _context.SaveChangesAsync(cancellationToken);

            return newBookshelf;
        }

        public async Task<Bookshelf?> GetBookshelfById(int id, CancellationToken cancellationToken)
        {
            return await _context.Bookshelves
                .AsNoTracking()
                .Include(b => b.BookshelfBooks)
                .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
        }

        public async Task<List<Bookshelf>> GetBookshelvesByUserId(string userId, CancellationToken cancellationToken)
        {
            return await _context.Bookshelves
                .AsNoTracking()
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<BookshelfBook> AddBookToBookshelf(int bookshelfId, string bookId, string title, string thumbnailUrl, CancellationToken cancellationToken)
        {
            var alreadyExists = await _context.BookshelfBooks
                .AsNoTracking()
                .AnyAsync(bb => bb.BookshelfId == bookshelfId && bb.VolumeId == bookId, cancellationToken);

            if (alreadyExists)
            {
                throw new BookshelfBookConflictException(bookshelfId, bookId);
            }

            var bookshelfBook = new BookshelfBook
            {
                BookshelfId = bookshelfId,
                VolumeId = bookId,
                Title = title,
                ThumbnailUrl = thumbnailUrl
            };

            _context.BookshelfBooks.Add(bookshelfBook);

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
                return bookshelfBook;
            }
            catch (DbUpdateException ex)
            {
                throw new BookshelfBookConflictException(bookshelfId, bookId, ex);
            }
        }
    }
}