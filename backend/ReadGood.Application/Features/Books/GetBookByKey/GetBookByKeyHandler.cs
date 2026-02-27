using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using ReadGood.Infrastructure.Interfaces;

namespace ReadGood.Application.Features.Books.GetBookByKey
{
    public class GetBookByKeyHandler : IRequestHandler<GetBookByKeyQuery, GetBookByKeyDto>
    {
        private readonly IGoogleBooksAPI googleBooksAPI;

        public GetBookByKeyHandler(IGoogleBooksAPI googleBooksAPI)
        {
            this.googleBooksAPI = googleBooksAPI;
        }

        public async Task<GetBookByKeyDto> Handle(GetBookByKeyQuery request, CancellationToken cancellationToken)
        {
            var book = await googleBooksAPI.GetBookById(request.Key, cancellationToken);

            return new GetBookByKeyDto
            {
                Book = book
            };
        }
    }
}