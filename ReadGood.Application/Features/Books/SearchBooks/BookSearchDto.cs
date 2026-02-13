using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReadGood.Domain.Entities;

namespace ReadGood.Application.Features.Books.SearchBooks
{
    public class BookSearchDto
    {
        public IEnumerable<BookSearchItem> Results { get; set; } = [];
    }
}