using GeneralLabSolutions.Domain.Entities;
using GeneralLabSolutions.Domain.Enums;
using GeneralLabSolutions.Domain.DTOs.DtosConsolidados;
using GeneralLabSolutions.Domain.Interfaces;
using GeneralLabSolutions.InfraStructure.Data;
using GeneralLabSolutions.InfraStructure.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace GeneralLabSolutions.InfraStructure.Repository
{
    public class ConsolidadoVendedorRepository : GenericRepository<Vendedor, Guid>, IConsolidadoVendedorRepository
    {
        private readonly IQueryGenericRepository<Vendedor, Guid> _queryRepository;
        private readonly IQueryGenericRepository<Pedido, Guid> _pedidoRepository;

        public ConsolidadoVendedorRepository(AppDbContext context,
                                             IQueryGenericRepository<Vendedor, Guid> queryRepository,
                                             IQueryGenericRepository<Pedido, Guid> pedidoRepository)
            : base(context)
        {
            _queryRepository = queryRepository;
            _pedidoRepository = pedidoRepository;
        }

        public async Task<IEnumerable<Vendedor>> GetAllVendedoresAsync()
        {
            return await _queryRepository.GetAllAsync();
        }

        public async Task<VendedorConsolidadoDto?> ObterConsolidadoVendedorPorIdAsync(Guid vendedorId)
        {
            // Obter o vendedor pelo Id
            var vendedor = await _queryRepository.GetByIdAsync(vendedorId);

            if (vendedor == null)
                return null;

            // Buscar todos os pedidos (vendas) do vendedor, excluindo os cancelados
            var pedidosDoVendedor = await _pedidoRepository.SearchAsync(p => p.VendedorId == vendedor.Id && p.StatusDoPedido != StatusDoPedido.Cancelado);

            // Certificar-se de que os itens do pedido estão sendo carregados
            var vendasComItens = pedidosDoVendedor
                .Select(p => _context.Pedido
                    .Include(p => p.Itens) // Incluindo os itens da venda
                    .FirstOrDefault(x => x.Id == p.Id))
                .Where(p => p != null).ToList();

            // Preparar o consolidado para o vendedor
            var consolidado = new VendedorConsolidadoDto
            {
                VendedorId = vendedor.Id,
                Nome = vendedor.Nome,
                TotalDeVendas = vendasComItens.Count(),
                UltimaVenda = vendasComItens.Any() ? vendasComItens.Max(p => p.DataPedido) : (DateTime?)null,
                TicketMedioPorVenda = vendasComItens.Any() ? vendasComItens.Sum(p => CalcularValorTotalDaVenda(p)) / vendasComItens.Count() : 0,
                IntervaloMedioEntreVendas = CalcularIntervaloMedio(vendasComItens),

                // Histórico de Vendas
                HistoricoDeVendas = vendasComItens.Select(p => new VendaHistoricoDto
                {
                    Id = p.Id,
                    DataVenda = p.DataPedido,
                    ValorTotal = CalcularValorTotalDaVenda(p),
                    Status = p.StatusDoPedido.ToString()
                }).ToList()
            };

            consolidado.ValorTotalDeVendas = vendasComItens.Sum(v => CalcularValorTotalDaVenda(v));


            return consolidado;
        }

        public async Task<ItensVendaConsolidadoDto> ObterItensVenda(Guid vendaId)
        {
            var venda = await _context.Pedido
                .Include(p => p.Itens)
                    .ThenInclude(i => i.Produto)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == vendaId);

            if (venda == null)
            {
                return null; // ou lance uma exceção
            }

            return new ItensVendaConsolidadoDto
            {
                Itens = venda.Itens.Select(item => new ItemVendaDto
                {
                    NomeProduto = item.Produto.Descricao,
                    Quantidade = item.Quantidade,
                    ValorUnitario = item.ValorUnitario
                }).ToList(),
                QuantidadeTotalItens = venda.Itens.Sum(i => i.Quantidade),
                ValorTotalItens = venda.Itens.Sum(i => i.ValorUnitario * i.Quantidade) // Corrigido para calcular o valor total
            };
        }


        // Método para calcular o valor total de uma venda com base nos itens
        private decimal CalcularValorTotalDaVenda(Pedido venda)
        {
            return venda.Itens.Sum(i => i.ValorUnitario * i.Quantidade);
        }

        // Método para calcular o intervalo médio entre vendas
        private int CalcularIntervaloMedio(IEnumerable<Pedido> vendas)
        {
            if (!vendas.Any())
                return 0;

            var intervalos = vendas.OrderBy(p => p.DataPedido)
                                   .Select((p, i) => i > 0 ? (p.DataPedido - vendas.ElementAt(i - 1).DataPedido).Days : 0)
                                   .Skip(1)
                                   .ToList();

            return intervalos.Any() ? (int)intervalos.Average() : 0;
        }
    }
}
