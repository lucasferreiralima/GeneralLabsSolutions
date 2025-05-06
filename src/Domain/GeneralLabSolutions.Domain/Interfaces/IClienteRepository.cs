using GeneralLabSolutions.Domain.Entities;
using GeneralLabSolutions.Domain.Extensions.Helpers.Generics;

namespace GeneralLabSolutions.Domain.Interfaces
{
	public interface IClienteRepository : IGenericRepository<Cliente, Guid>
	{
        Task<bool> TemCliente(Guid id);

        Task<PagedResult<Cliente>> ObterTodosPaginado(int pageIndex, int pageSize, string? query = null);
    }
}
