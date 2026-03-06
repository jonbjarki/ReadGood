using MediatR;
using ReadGood.Infrastructure.Interfaces;

namespace ReadGood.Application.Features.Books.SearchBooks
{
    public class SearchBooksHandler : IRequestHandler<SearchBooksQuery, BookSearchDto>
    {
        private readonly IGoogleBooksAPI _googleBooksAPI;

        public SearchBooksHandler(IGoogleBooksAPI googleBooksAPI)
        {
            _googleBooksAPI = googleBooksAPI;
        }

        public async Task<BookSearchDto> Handle(SearchBooksQuery request, CancellationToken cancellationToken)
        {
            var search = await _googleBooksAPI.Search(request.Title, cancellationToken, request.Author, request.Subject, request.Page, request.PageSize);
                
            return new BookSearchDto
            {
                Data = search
            };
        }
    }
}