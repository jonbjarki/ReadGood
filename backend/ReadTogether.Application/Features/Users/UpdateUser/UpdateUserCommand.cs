using MediatR;

namespace ReadTogether.Application.Features.Users.UpdateUser
{
    public record UpdateUserCommand(string Id, string? UserName, string? Email) : IRequest<UpdateUserDto?>;
}