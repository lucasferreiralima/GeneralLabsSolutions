namespace GeneralLabSolutions.WebAPI.DTOs
{
    public class PedidoItemDto
    {
        public PedidoDto? Pedido { get; set; }
        public virtual ICollection<ItemPedidoDto>? Items { get; set; }
        public virtual ICollection<ProdutoDto>? Produtos { get; set; }
    }
}
