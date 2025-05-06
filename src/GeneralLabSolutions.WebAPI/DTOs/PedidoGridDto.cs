using GeneralLabSolutions.Domain.Entities;
using GeneralLabSolutions.Domain.Enums;

namespace GeneralLabSolutions.WebAPI.DTOs
{
    public class PedidoGridDto
    {
        public Guid Id { get; set; }
        public Guid ClienteId { get; set; }
        public Guid VendedorId { get; set; }
        public DateTime DataPedido { get; set; } = DateTime.Now;
        public StatusDoPedido StatusDoPedido { get; set; }
            = StatusDoPedido.Orcamento;

    }
}
