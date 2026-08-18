using MediatR;

namespace ReadTogether.Application.Features.Books.GetBookById
{
    public record GetBookByIdQuery(string Id) : IRequest<GetBookByIdDto>;
}