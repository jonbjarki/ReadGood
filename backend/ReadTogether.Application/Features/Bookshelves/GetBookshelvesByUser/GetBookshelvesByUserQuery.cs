using MediatR;

namespace ReadTogether.Application.Features.Bookshelves.GetBookshelvesByUser
{
    public record GetBookshelvesByUserQuery(string UserName) : IRequest<List<GetBookshelvesByUserDto>?>;
}
