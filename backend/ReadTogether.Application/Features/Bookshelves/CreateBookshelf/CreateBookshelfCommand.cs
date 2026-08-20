using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace ReadTogether.Application.Features.Bookshelves.CreateBookshelf
{
    public record CreateBookshelfCommand(string Name, string UserId) : IRequest<CreateBookshelfDto>;
}