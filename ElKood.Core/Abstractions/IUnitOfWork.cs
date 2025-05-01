using ElKood.Core.Entities;
using ElKood.Core.Entities.Identity;

namespace ElKood.Core.Abstractions
{
    public interface IUnitOfWork
    {
        IRepository<TaskCategory> TaskCategories { get; }

        IRepository<ToDoTask> Tasks { get; }

        IRepository<AppLog> AppLogs { get; }

        IRepository<ApplicationUser> Users { get; }

        Task<int> SaveChangesAsync();

        void Dispose();
    }
}