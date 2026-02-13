using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace ReadGood.Application.Features.Books.GetBookById
{
    public record GetBookByIdQuery(int Id) : IRequest<BookDetailsDto>;
}