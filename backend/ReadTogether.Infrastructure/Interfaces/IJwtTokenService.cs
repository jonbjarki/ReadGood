using ReadTogether.Domain.Entities;
using ReadTogether.Infrastructure.Dtos;

namespace ReadTogether.Infrastructure.Interfaces
{
    public interface IJwtTokenService
    {
        public JwtTokenDto GenerateToken(ApplicationUser user);
    }
}