using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReadGood.Domain.Entities;

namespace ReadGood.Infrastructure.Interfaces
{
    public interface IBookshelfRepository
    {
        Task<Bookshelf> CreateBookshelf(string name, string userId, CancellationToken cancellationToken);
        Task<Bookshelf?> GetBookshelfById(int id, CancellationToken cancellationToken);
    }
}