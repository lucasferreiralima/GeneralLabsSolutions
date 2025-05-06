﻿using GeneralLabSolutions.Domain.Entities;
using GeneralLabSolutions.Domain.Enums;
using GeneralLabSolutions.Domain.DTOs.DtosConsolidados;
using GeneralLabSolutions.Domain.Interfaces;
using GeneralLabSolutions.InfraStructure.Data;
using GeneralLabSolutions.InfraStructure.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace GeneralLabSolutions.InfraStructure.Repository
{
    public class ConsolidadoClienteRepository : GenericRepository<Cliente, Guid>, IConsolidadoClienteRepository
    {
        private readonly IQueryGenericRepository<Cliente, Guid> _queryRepository;
        private readonly IQueryGenericRepository<Pedido, Guid> _pedidoRepository;

        public ConsolidadoClienteRepository(AppDbContext context,
                                            IQueryGenericRepository<Cliente, Guid> queryRepository,
                                            IQueryGenericRepository<Pedido, Guid> pedidoRepository)
            : base(context)
        {
            _queryRepository = queryRepository;
            _pedidoRepository = pedidoRepository;
        }

        public async Task<IEnumerable<Cliente>> GetAllClientesAsync()
        {
            return await _queryRepository.GetAllAsync();
        }

        // Método principal que retorna o consolidado de um cliente específico
        public async Task<ClienteConsolidadoDto?> ObterConsolidadoClientePorIdAsync(Guid clienteId)
        {
            // Obter o cliente pelo Id
            var cliente = await _queryRepository.GetByIdAsync(clienteId);

            if (cliente == null)
                return null;

            // Buscar todos os pedidos do cliente, excluindo os cancelados
            var pedidosDoCliente = await _pedidoRepository.SearchAsync(p => p.ClienteId == cliente.Id && p.StatusDoPedido != StatusDoPedido.Cancelado);

            // Certificar-se de que os itens do pedido estão sendo carregados
            var pedidosComItens = pedidosDoCliente
                .Select(p => _context.Pedido
                    .Include(p => p.Itens) // Incluindo os itens do pedido
                    .FirstOrDefault(x => x.Id == p.Id))
                .Where(p => p != null).ToList();

            // Preparar o consolidado para o cliente
            var consolidado = new ClienteConsolidadoDto
            {
                ClienteId = cliente.Id,
                Nome = cliente.Nome,
                QuantidadeDePedidos = pedidosComItens.Count(),
                UltimaCompraDesteCliente = pedidosComItens.Any() ? pedidosComItens.Max(p => p.DataPedido) : (DateTime?)null,
                TicketMedioPorPedido = pedidosComItens.Any() ? pedidosComItens.Sum(p => CalcularValorTotalDoPedido(p)) / pedidosComItens.Count() : 0,
                IntervaloMedioEntrePedidos = CalcularIntervaloMedio(pedidosComItens),
                HistoricoDePedidos = pedidosComItens.Select(p => new PedidoHistoricoDto
                {
                    Id = p.Id,
                    DataPedido = p.DataPedido,
                    ValorTotal = CalcularValorTotalDoPedido(p),
                    Status = p.StatusDoPedido.ToString()
                }).ToList(),
                ValorTotalDeCompras = pedidosComItens.Sum(p => CalcularValorTotalDoPedido(p)) // Adiciona o valor total de compras
            };


            return consolidado;
        }

        public async Task<Cliente?> ObterClienteComPedidosEItensEProdutoEFornecedor(Guid clienteId)
        {
            var cliente = await _context.Cliente
                .Include(c => c.Pedidos)
                    .ThenInclude(p => p.Itens)
                        .ThenInclude(i => i.Produto)
                            .ThenInclude(prod => prod.Fornecedor) // Inclui os dados do fornecedor
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == clienteId);

            return cliente;
        }


        public async Task<Pedido?> ObterPedidoPorClienteIdComItensEDadosDoFornecedor(Guid clienteId)
        {
            var pedido = await _context.Pedido
                    .Include(p => p.Itens)
                        .ThenInclude(i => i.Produto)
                            .ThenInclude(prod => prod.Fornecedor) // Inclui os dados do fornecedor
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == clienteId);

            return pedido;
        }


        public async Task<ItensPedidoConsolidadoDto> ObterItensPedido(Guid pedidoId)
        {
            var pedido = await ObterPedidoPorClienteIdComItensEDadosDoFornecedor(pedidoId);

            if (pedido == null)
            {
                return new ItensPedidoConsolidadoDto();
            }

            return new ItensPedidoConsolidadoDto
            {
                Itens = pedido.Itens.Select(item => new ItemPedidoDto
                {
                    NomeProduto = item.Produto.Descricao,
                    Quantidade = item.Quantidade,
                    ValorUnitario = item.ValorUnitario
                }).ToList(),
                QuantidadeTotalItens = pedido.Itens.Sum(i => i.Quantidade),
                ValorTotalItens = pedido.Itens.Sum(i => i.ValorUnitario * i.Quantidade)
            };
        }



        // Método para calcular o valor total de um pedido com base nos itens
        private decimal CalcularValorTotalDoPedido(Pedido pedido)
        {
            return pedido.Itens.Sum(i => i.ValorUnitario * i.Quantidade);
        }

        // Método para calcular o intervalo médio entre pedidos
        private int CalcularIntervaloMedio(IEnumerable<Pedido> pedidos)
        {
            if (!pedidos.Any())
                return 0;

            var intervalos = pedidos.OrderBy(p => p.DataPedido)
                                    .Select((p, i) => i > 0 ? (p.DataPedido - pedidos.ElementAt(i - 1).DataPedido).Days : 0)
                                    .Skip(1)
                                    .ToList();

            return intervalos.Any() ? (int)intervalos.Average() : 0;
        }
    }
}
