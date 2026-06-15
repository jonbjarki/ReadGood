using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReadGood.Domain.Contexts;
using ReadGood.Domain.Entities;
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

        public async Task<Bookshelf> CreateBookshelf(string name, string userId, CancellationToken cancellationToken)
        {
            var newBookshelf = new Bookshelf
            {
                Name = name,
                UserId = userId
            };

            _context.Bookshelves.Add(newBookshelf);
            await _context.SaveChangesAsync(cancellationToken);

            return newBookshelf;
        }

        public async Task<Bookshelf?> GetBookshelfById(int id, CancellationToken cancellationToken)
        {
            return await _context.Bookshelves
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
        }
    }
}