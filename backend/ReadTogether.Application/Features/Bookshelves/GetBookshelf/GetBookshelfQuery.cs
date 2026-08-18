using MediatR;

namespace ReadTogether.Application.Features.Bookshelves.GetBookshelf
{
    public record GetBookshelfQuery(int Id) : IRequest<GetBookshelfDto?>;
}
