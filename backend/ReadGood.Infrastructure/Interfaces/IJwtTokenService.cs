using ReadGood.Domain.Entities;
using ReadGood.Infrastructure.Dtos;

namespace ReadGood.Infrastructure.Interfaces
{
    public interface IJwtTokenService
    {
        public JwtTokenDto GenerateToken(ApplicationUser user);
    }
}