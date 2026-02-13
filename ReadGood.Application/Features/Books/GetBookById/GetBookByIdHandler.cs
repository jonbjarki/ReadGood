using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using ReadGood.Infrastructure.Interfaces;

namespace ReadGood.Application.Features.Books.GetBookById
{
    public class GetBookByIdHandler : IRequestHandler<GetBookByIdQuery, BookDetailsDto>
    {
        private readonly IOpenLibraryAPI _openLibraryApi;

        public GetBookByIdHandler(IOpenLibraryAPI openLibraryApi)
        {
            _openLibraryApi = openLibraryApi;
        }

        public async Task<BookDetailsDto> Handle(GetBookByIdQuery request, CancellationToken ct)
        {
            throw new NotImplementedException();
            return new BookDetailsDto();
        }
    }
}