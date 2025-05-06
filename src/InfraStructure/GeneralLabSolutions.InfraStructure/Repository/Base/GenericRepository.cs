using GeneralLabSolutions.Domain.Interfaces;
using GeneralLabSolutions.InfraStructure.Data;

namespace GeneralLabSolutions.InfraStructure.Repository.Base
{
    public abstract class GenericRepository<T, TKey> : IGenericRepository<T, TKey> where T : class, new()
    {
        protected readonly AppDbContext _context;


		public GenericRepository(AppDbContext context)
        {
            _context = context;
        }


        public IUnitOfWork UnitOfWork => _context;


        public async Task AddAsync(T obj)
        {
            _context.Set<T>().Add(obj);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(T obj)
        {
            _context.Set<T>().Remove(obj);
            await Task.CompletedTask;
        }

        public async Task UpdateAsync(T obj)
        {
            _context.Set<T>().Update(obj);
            await Task.CompletedTask;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

    }
}
