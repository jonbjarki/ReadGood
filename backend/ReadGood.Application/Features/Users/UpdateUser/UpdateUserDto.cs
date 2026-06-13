namespace ReadGood.Application.Features.Users.UpdateUser
{
    public class UpdateUserDto
    {
        public string Id { get; set; } = null!;
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public bool ProfileCompleted { get; set; }
    }
}