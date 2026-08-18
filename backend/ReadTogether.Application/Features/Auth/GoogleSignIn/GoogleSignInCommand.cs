using MediatR;

namespace ReadTogether.Application.Features.Auth.GoogleSignIn
{
    public sealed record GoogleSignInCommand(string idToken) : IRequest<GoogleSignInResult>;
}