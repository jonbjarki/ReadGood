using MediatR;
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
            var search = await _openLibraryAPI.Search(request.Title, cancellationToken, request.Page, request.PageSize);
            if (search == null)
            {
                throw new Exception();
            }
            
            return new BookSearchDto
            {
                Data = search
            };
        }
    }
}