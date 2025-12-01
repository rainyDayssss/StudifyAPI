namespace StudifyAPI.Features.Users
{
    public class UserReadDTO
    {
        public int Id { get; set; }
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int Level { get; set; }
        public int Experience { get; set; }
        public int CurrentStreak { get; set; }
    }
}
