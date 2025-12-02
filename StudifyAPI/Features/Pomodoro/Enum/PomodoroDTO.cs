namespace StudifyAPI.Features.Pomodoro.Enum
{
    public class PomodoroDTO
    {
        public int DurationMinutes { get; set; } = 25; // default duration is 25 minutes
        public bool IsCompleted { get; set; } = false; // default is not completed
        public PomodoroType PomodoroType { get; set; } = PomodoroType.Work; // deafault value is work (25 mins)
    }
}
