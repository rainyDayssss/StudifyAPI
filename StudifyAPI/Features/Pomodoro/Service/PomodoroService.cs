using StudifyAPI.Features.Pomodoro.Enum;

namespace StudifyAPI.Features.Pomodoro.Service
{
    public class PomodoroService : IPomodoroService
    {
        public List<PomodoroDTO> GetDefaultPomodoros()
        {
            return new List<PomodoroDTO>()
            {
                new() { DurationMinutes = 25, IsCompleted = false, PomodoroType = PomodoroType.Work },
                new() { DurationMinutes = 5, IsCompleted = false, PomodoroType = PomodoroType.ShortBreak },
                new() { DurationMinutes = 15, IsCompleted = false, PomodoroType = PomodoroType.LongBreak },
            };
        }
    }
}
