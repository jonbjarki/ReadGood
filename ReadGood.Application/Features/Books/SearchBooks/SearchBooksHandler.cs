using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using ReadGood.Domain.Entities;
using ReadGood.Infrastructure.Interfaces;

namespace ReadGood.Application.Features.Books.SearchBooks
{
    public class SearchBooksHandler : IRequestHandler<SearchBooksQuery, BookSearchDto>
    {
        private readonly IOpenLibraryAPI _openLibraryAPI;

        public SearchBooksHandler(IOpenLibraryAPI openLibraryAPI)
        {
            _openLibraryAPI = openLibraryAPI;
        }

        public async Task<BookSearchDto> Handle(SearchBooksQuery request, CancellationToken cancellationToken)
        {
            var search = await _openLibraryAPI.Search(request.Title) ?? throw new Exception();
            if (search == null)
            {
                throw new Exception();
            }
            
            return new BookSearchDto
            {
                Results = search
            };
        }
    }
}