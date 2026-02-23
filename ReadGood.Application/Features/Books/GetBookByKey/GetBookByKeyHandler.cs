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
        private readonly IOpenLibraryAPI openLibraryAPI;

        public GetBookByKeyHandler(IOpenLibraryAPI openLibraryAPI)
        {
            this.openLibraryAPI = openLibraryAPI;
        }

        public async Task<GetBookByKeyDto> Handle(GetBookByKeyQuery request, CancellationToken cancellationToken)
        {
            var book = await openLibraryAPI.GetBookByKey(request.Key, cancellationToken);
            if (book is null)
            {
                throw new Exception();
            }

            return new GetBookByKeyDto
            {
                Book = book
            };
        }
    }
}