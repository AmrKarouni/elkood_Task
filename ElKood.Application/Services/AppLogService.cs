using ElKood.Application.Interfaces;
using ElKood.Core.Abstractions;
using ElKood.Core.Entities;
using ElKood.Core.Enums;

namespace ElKood.Application.Services
{
    public class AppLogService : IAppLogService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AppLogService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task LogEvent(LogEventTypes eventType, string userName, string message)
        {
            try
            {
                await _unitOfWork.AppLogs.AddAsync(new AppLog { Username = string.IsNullOrEmpty(userName) ? "Anonymous" : userName, EventType = eventType, Message = message });
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.AppLogs.AddAsync(new AppLog { Username = "system", EventType = LogEventTypes.Exception, Message = "Error In Adding Log." });
                await _unitOfWork.SaveChangesAsync();
            }

        }
    }
}
