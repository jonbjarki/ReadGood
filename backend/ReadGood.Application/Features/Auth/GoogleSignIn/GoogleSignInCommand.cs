using MediatR;

namespace ReadGood.Application.Features.Auth.GoogleSignIn
{
    public sealed record GoogleSignInCommand(string idToken) : IRequest<GoogleSignInResult>;
}