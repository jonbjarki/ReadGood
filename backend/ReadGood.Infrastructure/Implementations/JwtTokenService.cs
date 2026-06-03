using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using ReadGood.Domain.Configuration;
using ReadGood.Domain.Entities;
using ReadGood.Infrastructure.Dtos;
using ReadGood.Infrastructure.Interfaces;

namespace ReadGood.Infrastructure.Implementations
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtConfiguration jwtConfiguration;

        public JwtTokenService(IOptions<JwtConfiguration> options)
        {
            this.jwtConfiguration = options.Value;
        }
        public JwtTokenDto GenerateToken(ApplicationUser user)
        {
            // Token will be signed using symmetric encryption shared key 
            var key = Encoding.UTF8.GetBytes(jwtConfiguration.Key);
            var securityKey = new SymmetricSecurityKey(key);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var handler = new JsonWebTokenHandler { SetDefaultTimesOnTokenCreation = false };

            // Configures Token Payload
            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = jwtConfiguration.Issuer,
                Subject = new ClaimsIdentity(
                [
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email!),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Role, "User") // TODO: Change once roles are implemented.
                ]),
                Audience = jwtConfiguration.Audience,
                Expires = DateTime.UtcNow.AddHours(jwtConfiguration.ExpiresInHours),
                SigningCredentials = credentials
            };

            // Creates and signs the token
            var token = handler.CreateToken(descriptor);
            return new JwtTokenDto
            {
                Token = token,
                ExpiresAt = (DateTime)descriptor.Expires
            };
        }
    }
}