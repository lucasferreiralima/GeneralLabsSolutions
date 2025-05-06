using System.Linq.Expressions;
using GeneralLabSolutions.Domain.Interfaces;

namespace GeneralLabSolutions.Domain.Interfaces
{
    public interface IGenericRepository<T, TKey> : IDisposable where T : class
    {

        IUnitOfWork UnitOfWork { get; }

        Task AddAsync(T obj);
        Task DeleteAsync(T obj);
        Task UpdateAsync(T obj);

    }
}
