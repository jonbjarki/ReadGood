using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace ReadGood.Application.Features.Books.GetBookByKey
{
    public record GetBookByKeyQuery(string Key) : IRequest<GetBookByKeyDto>;
}