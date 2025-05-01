using ElKood.Core.Abstractions;
using ElKood.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace elkood_Task.Repository
{
    public class Repository<T> : IRepository<T>
       where T : class
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<T> _entityTable;

        public Repository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _entityTable = _dbContext.Set<T>();
        }

        public IQueryable<T> GetAll()
        {
            return _entityTable.Select(t => t);
        }

        public async Task<T> AddAsync(T entity)
        {
            await _entityTable.AddAsync(entity);
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _entityTable.AddRangeAsync(entities);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _entityTable.RemoveRange(entities);
        }

        public async Task UpdateAsync(T entity)
        {
            await Task.Run(() => _entityTable.Update(entity));
        }
    }
}
