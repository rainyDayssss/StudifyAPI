namespace StudifyAPI.Features.Users
{
    public class UserCreateDTO
    {
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
