using GeneralLabSolutions.Domain.Entities;
using GeneralLabSolutions.Domain.DTOs.DtosConsolidados;

namespace GeneralLabSolutions.Domain.Interfaces
{
    public interface IConsolidadoFornecedorRepository
    {
        Task<FornecedorConsolidadoDto?> ObterConsolidadoFornecedorPorIdAsync(Guid fornecedorId);
        Task<IEnumerable<Fornecedor>> ObterTodosFornecedoresAsync();
    }
}