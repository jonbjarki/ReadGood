using MediatR;

namespace ReadGood.Application.Features.Bookshelves.GetBookshelvesByUser
{
    public record GetBookshelvesByUserQuery(string UserName) : IRequest<List<GetBookshelvesByUserDto>?>;
}
