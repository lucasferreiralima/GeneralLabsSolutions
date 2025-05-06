using GeneralLabSolutions.Domain.Entities;
using GeneralLabSolutions.Domain.Enums;

namespace GeneralLabSolutions.WebAPI.DTOs
{
    public class PedidoDto
    {
        public Guid Id { get; set; }
        public Guid ClienteId { get; set; }
        public Guid VendedorId { get; set; }
        public DateTime DataPedido { get; set; } = DateTime.Now;
        public StatusDoPedido StatusDoPedido { get; set; }
            = StatusDoPedido.Orcamento;

        public virtual ClienteDto? Cliente { get; set; }
        public virtual VendedorDto? Vendedor { get; set; }

        // Relacionamento
        public virtual ICollection<ItemPedidoDto> Itens { get; set; }
            = new List<ItemPedidoDto>();

        public Guid? VoucherId { get; set; }

        public virtual VoucherDto? Voucher { get; set; }

    }
}
