using StudifyAPI.Features.Pomodoro.Enum;
namespace StudifyAPI.Features.Pomodoro.Service
{
    public interface IPomodoroService
    {
        public List<PomodoroDTO> GetDefaultPomodoros();
    }
}
