using GeneralLabSolutions.Domain.Enums;

namespace GeneralLabSolutions.WebAPI.DTOs
{
    public class VoucherDto
    {
        public Guid Id { get; set; }
        public string? Codigo { get; set; }
        public decimal? Percentual { get; set; }
        public decimal? ValorDesconto { get; set; }
        public int Quantidade { get; set; }
        public TipoDescontoVoucher TipoDescontoVoucher { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataUtilizacao { get; set; }
        public DateTime DataValidade { get; set; }
        public bool Ativo { get; set; }
        public bool Utilizado { get; set; }

        // EF Relationship
        public ICollection<PedidoDto> Pedidos { get; set; }
               = new List<PedidoDto>();
    }
}
