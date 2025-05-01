using ElKood.Core.Enums;

namespace ElKood.Application.Interfaces
{
    public interface IAppLogService
    {
        public Task LogEvent(LogEventTypes eventType, string userName, string message);
    }
}
