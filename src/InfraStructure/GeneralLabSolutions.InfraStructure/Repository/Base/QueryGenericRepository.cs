using System.Linq.Expressions;
using GeneralLabSolutions.Domain.Interfaces;
using GeneralLabSolutions.InfraStructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GeneralLabSolutions.InfraStructure.Repository.Base
{
    public class QueryGenericRepository<T, TKey> : IQueryGenericRepository<T, TKey> where T : class, new()
    {
        private readonly AppDbContext _context;

        public QueryGenericRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<T?> GetByIdAsync(TKey id)
        {
            return await Task.FromResult(await _context.Set<T>().FindAsync(id));
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<T>> SearchAsync(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null)
            {
                return await _context.Set<T>().AsNoTracking().ToListAsync();
            }
            return await _context.Set<T>().AsNoTracking().Where(predicate).ToListAsync();
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().AsNoTracking().AnyAsync(predicate);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

    }
}
