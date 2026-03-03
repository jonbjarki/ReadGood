using MediatR;

namespace ReadGood.Application.Features.Books.GetBookById
{
    public record GetBookByIdQuery(string Id) : IRequest<GetBookByIdDto>;
}