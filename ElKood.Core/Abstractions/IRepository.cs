namespace ElKood.Core.Abstractions
{
    public interface IRepository<T>
        where T : class
    {
        IQueryable<T> GetAll();

        Task<T> AddAsync(T entity);

        Task AddRangeAsync(IEnumerable<T> entities);

        Task UpdateAsync(T entity);

        void RemoveRange(IEnumerable<T> entities);
    }
}
