using Google.Apis.Auth;
using Microsoft.Extensions.Options;
using ReadTogether.API.Configuration;
using ReadTogether.Domain.DTOs;
using ReadTogether.Infrastructure.Interfaces;

namespace ReadTogether.Infrastructure.Implementations
{
    public class GoogleTokenValidator : IGoogleTokenValidator
    {
        private readonly GoogleConfiguration googleConfiguration;

        public GoogleTokenValidator(IOptions<GoogleConfiguration> options)
        {
            this.googleConfiguration = options.Value;
        }

        public async Task<GoogleUserInfoDto> ValidateToken(
        string idToken,
        CancellationToken cancellationToken)
        {
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(
                    idToken,
                    new GoogleJsonWebSignature.ValidationSettings
                    {
                        Audience = [googleConfiguration.ClientId]
                    });

                return new GoogleUserInfoDto
                {
                    Email = payload.Email,
                    Name = payload.Name,
                    EmailVerified = payload.EmailVerified,
                    PictureUrl = payload.Picture,
                    Subject = payload.Subject
                };
            }
            catch (InvalidJwtException ex)
            {
                // TODO: Handle error by throwing a custom exception
                Console.Error.WriteLine(ex.Data);
                throw new Exception("Invalid token");
            }

        }
    }
}