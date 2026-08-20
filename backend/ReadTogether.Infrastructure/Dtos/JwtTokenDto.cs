namespace ReadTogether.Infrastructure.Dtos
{
    public class JwtTokenDto
    {
        public required string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}