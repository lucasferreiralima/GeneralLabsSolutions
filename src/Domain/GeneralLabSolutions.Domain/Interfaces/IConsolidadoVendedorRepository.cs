using GeneralLabSolutions.Domain.Entities;
using GeneralLabSolutions.Domain.DTOs.DtosConsolidados;

namespace GeneralLabSolutions.Domain.Interfaces
{
    public interface IConsolidadoVendedorRepository
    {
        /// <summary>
        /// Retorna o consolidado de um vendedor específico
        /// </summary>
        Task<VendedorConsolidadoDto?> ObterConsolidadoVendedorPorIdAsync(Guid vendedorId);

        /// <summary>
        /// Retorna todos os vendedores
        /// </summary>
        Task<IEnumerable<Vendedor>> GetAllVendedoresAsync();

        /// <summary>
        /// Retorna os itens de uma venda específica
        /// </summary>
        Task<ItensVendaConsolidadoDto> ObterItensVenda(Guid vendaId);
    }
}
