using GeneralLabSolutions.Domain.Entities;

namespace GeneralLabSolutions.Domain.DTOs.DtosConsolidados
{
    public class FornecedorConsolidadoDto
    {
        public Guid FornecedorId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int TotalDeProdutos { get; set; }
        public decimal ValorTotalGerado { get; set; }
        public List<ProdutoHistoricoDto> ProdutosMaisVendidos { get; set; } = new List<ProdutoHistoricoDto>();
        public List<CategoriaProduto> CategoriasMaisPopulares { get; set; } = new List<CategoriaProduto>();
        public double TaxaDevolucao { get; set; }
        // Reaproveitando do Consolidado de Vendedor
        public List<VendaHistoricoDto> HistoricoDeVendas { get; set; } = new List<VendaHistoricoDto>();
    }
}