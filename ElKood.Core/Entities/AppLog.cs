using ElKood.Core.Enums;

namespace ElKood.Core.Entities
{
    public class AppLog
    {
        public int Id { get; set; }

        public LogEventTypes EventType { get; set; } = LogEventTypes.Event;

        public string Username { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
