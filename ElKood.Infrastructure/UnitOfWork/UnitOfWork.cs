using ElKood.Core.Abstractions;
using ElKood.Core.Entities;
using ElKood.Core.Entities.Identity;
using elkood_Task.Repository;

namespace ElKood.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;

        public IRepository<TaskCategory> TaskCategories { get; }

        public IRepository<ToDoTask> Tasks { get; }

        public IRepository<AppLog> AppLogs { get; }

        public IRepository<ApplicationUser> Users { get; }

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            TaskCategories = new Repository<TaskCategory>(_dbContext);
            Tasks = new Repository<ToDoTask>(_dbContext);
            AppLogs = new Repository<AppLog>(_dbContext);
            Users = new Repository<ApplicationUser>(_dbContext);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
