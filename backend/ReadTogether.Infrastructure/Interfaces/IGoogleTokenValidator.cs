using ReadTogether.Domain.DTOs;

namespace ReadTogether.Infrastructure.Interfaces
{
    public interface IGoogleTokenValidator
    {
        public Task<GoogleUserInfoDto> ValidateToken(string idToken, CancellationToken cancellationToken);
    }
}