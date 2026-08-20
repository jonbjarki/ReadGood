namespace ReadTogether.Application.Features.Auth.GoogleSignIn
{
    public class GoogleSignInResult
    {
        public required string UserId { get; set; }
        public required string Email { get; set; }
        public string UserName { get; set; } = "";
        public required string JwtToken { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}