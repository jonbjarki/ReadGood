using MediatR;

namespace ReadGood.Application.Features.Bookshelves.GetBookshelf
{
    public record GetBookshelfQuery(int Id) : IRequest<GetBookshelfDto?>;
}
