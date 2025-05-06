using GeneralLabSolutions.Domain.Entities;
using GeneralLabSolutions.Domain.DTOs.DtosConsolidados;
using GeneralLabSolutions.Domain.Interfaces;
using GeneralLabSolutions.InfraStructure.Data;
using GeneralLabSolutions.InfraStructure.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace GeneralLabSolutions.InfraStructure.Repository
{
    public class ConsolidadoFornecedorRepository : GenericRepository<Fornecedor, Guid>, IConsolidadoFornecedorRepository
    {
        private readonly IQueryGenericRepository<Fornecedor, Guid> _queryRepository;
        private readonly IQueryGenericRepository<Pedido, Guid> _pedidoRepository;

        public ConsolidadoFornecedorRepository(AppDbContext context,
                                                IQueryGenericRepository<Fornecedor, Guid> queryRepository,
                                                IQueryGenericRepository<Pedido, Guid> pedidoRepository) : base(context)
        {
            _queryRepository = queryRepository;
            _pedidoRepository = pedidoRepository;
        }


        public async Task<FornecedorConsolidadoDto?> ObterConsolidadoFornecedorPorIdAsync(Guid fornecedorId)
        {
            // Começando pelos Itens de Pedido relacionados ao Fornecedor
            var itensDoFornecedor = await _context.ItemPedido
                .Include(i => i.Produto)
                    .Include(i => i.Pedido) // <- Incluindo o Pedido aqui!
                .Where(i => i.Produto.FornecedorId == fornecedorId)
                .ToListAsync();

            if (itensDoFornecedor.Count == 0)
                return null; // Ou retornar um DTO vazio, se preferir

            // Obtendo os produtos do fornecedor a partir dos Itens
            var produtosDoFornecedor = itensDoFornecedor
                .Select(i => i.Produto)
                .Distinct()
                .ToList();

            // Calculando o ValorTotalGerado a partir dos Itens
            var valorTotalGerado = itensDoFornecedor.Sum(i => i.Quantidade * i.ValorUnitario);

            // Obtendo o nome do Fornecedor diretamente do banco de dados
            var fornecedorNome = (await _queryRepository.GetByIdAsync(fornecedorId))?.Nome ?? string.Empty;

            return new FornecedorConsolidadoDto
            {
                FornecedorId = fornecedorId, // Já temos o ID do fornecedor
                Nome = fornecedorNome, // Usando o nome obtido diretamente do banco
                TotalDeProdutos = produtosDoFornecedor.Count,
                ValorTotalGerado = valorTotalGerado,
                ProdutosMaisVendidos = ObterProdutosMaisVendidos(fornecedorId),
                CategoriasMaisPopulares = ObterCategoriasMaisPopulares(fornecedorId),
                HistoricoDeVendas = itensDoFornecedor // Construindo o Histórico a partir dos Itens
                                        .Select(i => i.Pedido)
                                        .Distinct()
                                        .Select(p => new VendaHistoricoDto
                                        {
                                            Id = p.Id,
                                            DataVenda = p.DataPedido,
                                            ValorTotal = p.CalcularValorTotal(),
                                            Status = p.StatusDoPedido.ToString()
                                        }).ToList()
            };
        }


        private List<ProdutoHistoricoDto> ObterProdutosMaisVendidos(Guid fornecedorId)
        {
            // Começando pelos Itens de Pedido, filtrados pelo fornecedor
            var itens = _context.ItemPedido
                .Include(i => i.Produto) // Incluindo o Produto relacionado
                    .ThenInclude(p => p.CategoriaProduto) // E a Categoria do Produto
                .Where(i => i.Produto.FornecedorId == fornecedorId)
                .ToList();

            // Agrupando os itens por Produto
            return itens
                .GroupBy(i => i.Produto)
                .Select(g => new ProdutoHistoricoDto
                {
                    ProdutoId = g.Key.Id,
                    Nome = g.Key.Descricao ?? string.Empty,
                    QuantidadeVendida = g.Sum(i => i.Quantidade),
                    ValorTotalVendido = g.Sum(i => i.Quantidade * i.ValorUnitario)
                })
                .OrderByDescending(dto => dto.QuantidadeVendida)
                .Take(5)
                .ToList();
        }

        private List<CategoriaProduto>? ObterCategoriasMaisPopulares(Guid fornecedorId)
        {
            // Começando pelos Itens de Pedido, filtrados pelo fornecedor
            var itens = _context.ItemPedido
                .Include(i => i.Produto)
                    .ThenInclude(p => p.CategoriaProduto)
                .Where(i => i.Produto.FornecedorId == fornecedorId)
                .ToList();

            // Agrupando os itens por Categoria do Produto
            return itens
                .GroupBy(i => i.Produto.CategoriaProduto)
                .Select(g => new
                {
                    Categoria = g.Key ?? new CategoriaProduto(), // Usando ?? para garantir uma CategoriaProduto válida
                    QuantidadeTotal = g.Sum(x => x.Quantidade)
                })
                .OrderByDescending(x => x.QuantidadeTotal)
                .Take(5)
                .Select(x => x.Categoria)
                .ToList();
        }

        public async Task<IEnumerable<Fornecedor>> ObterTodosFornecedoresAsync()
        {
            return await _queryRepository.GetAllAsync();
        }
    }
}