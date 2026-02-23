using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace ReadGood.Application.Features.Books.SearchBooks
{
    public record SearchBooksQuery(string Title, int Page, int PageSize) : IRequest<BookSearchDto>;
}