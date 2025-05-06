using GeneralLabSolutions.Domain.Entities;
using GeneralLabSolutions.Domain.Extensions.Helpers.Generics;
using GeneralLabSolutions.Domain.Interfaces;
using GeneralLabSolutions.InfraStructure.Data;
using GeneralLabSolutions.InfraStructure.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace GeneralLabSolutions.InfraStructure.Repository
{
    public class ClienteRepository : GenericRepository<Cliente, Guid>, IClienteRepository
    {
        public ClienteRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> TemCliente(Guid id)
        {
            return _context.Set<Cliente>().Any(x => x.Id == id);
        }


        public async Task<PagedResult<Cliente>> ObterTodosPaginado(int pageIndex, int pageSize, string? query = null)
        {
            IEnumerable<Cliente> data = new List<Cliente>();
            var source = _context.Cliente.AsQueryable();

            data = query != null
                ? await source.Where(x => x.Nome.Contains(query)).OrderBy(x => x.Nome).ToListAsync()
                : await source.OrderBy(x => x.Nome).ToListAsync();

            var count = data.Count();
            data = data.Skip(pageSize * (pageIndex - 1)).Take(pageSize);

            var totalPages = (int)Math.Ceiling(count / (double)pageSize);

            return new PagedResult<Cliente>()
            {
                List = data,
                TotalPages = totalPages,
                TotalResults = count,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Query = query,
                HasPrevious = pageIndex > 1,
                HasNext = pageIndex < totalPages
            };


        }


    }
}
