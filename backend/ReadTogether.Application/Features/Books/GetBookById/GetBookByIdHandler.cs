using MediatR;
using ReadTogether.Infrastructure.Interfaces;

namespace ReadTogether.Application.Features.Books.GetBookById
{
    public class GetBookByIdHandler : IRequestHandler<GetBookByIdQuery, GetBookByIdDto>
    {
        private readonly IGoogleBooksAPI googleBooksAPI;

        public GetBookByIdHandler(IGoogleBooksAPI googleBooksAPI)
        {
            this.googleBooksAPI = googleBooksAPI;
        }

        public async Task<GetBookByIdDto> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
        {
            var book = await googleBooksAPI.GetBookById(request.Id, cancellationToken);

            return new GetBookByIdDto
            {
                Book = book
            };
        }
    }
}