using System.Linq.Expressions;

namespace GeneralLabSolutions.Domain.Interfaces
{
    public interface IQueryGenericRepository<T, TKey> : IDisposable where T : class
    {
        Task<T> GetByIdAsync(TKey id);

        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> SearchAsync(Expression<Func<T, bool>> predicate);

        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    }
}
