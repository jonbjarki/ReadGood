using ReadGood.Domain.DTOs;

namespace ReadGood.Infrastructure.Interfaces
{
    public interface IGoogleTokenValidator
    {
        public Task<GoogleUserInfoDto> ValidateToken(string idToken, CancellationToken cancellationToken);
    }
}