using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ReadGood.Application.Features.Books.SearchBooks;
using ReadGood.Domain.Entities;

namespace ReadGood.Infrastructure.Interfaces
{
    public interface IOpenLibraryAPI
    {
        Task<IEnumerable<BookSearchItem>?> Search(string title);
    }
}