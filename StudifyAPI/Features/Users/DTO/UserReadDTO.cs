namespace StudifyAPI.Features.Users.DTOs
{
    public class UserReadDTO
    {
        public int Id { get; set; }
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool IsOnline { get; set; }
        public int CurrentStreakDays { get; set; }

        public int NumberOfFriends {get; set; }
    }
}
