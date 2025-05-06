using GeneralLabSolutions.Domain.Entities;
using GeneralLabSolutions.Domain.DTOs.DtosConsolidados;

namespace GeneralLabSolutions.Domain.Interfaces
{
    public interface IConsolidadoClienteRepository : IGenericRepository<Cliente, Guid>
    {
        Task<IEnumerable<Cliente>> GetAllClientesAsync();
        Task<ClienteConsolidadoDto?> ObterConsolidadoClientePorIdAsync(Guid clienteId);
        Task<ItensPedidoConsolidadoDto> ObterItensPedido(Guid pedidoId);
        Task<Cliente?> ObterClienteComPedidosEItensEProdutoEFornecedor(Guid clienteId);

        Task<Pedido?> ObterPedidoPorClienteIdComItensEDadosDoFornecedor(Guid clienteId);


    }
}
