namespace GeneralLabSolutions.WebAPI.DTOs
{
    public class ItemPedidoDto
    {
        public Guid Id { get; set; }

        public Guid PedidoId { get; set; }

        public Guid ProdutoId { get; set; }
        public int Quantidade { get; set; } = 0;
        public decimal ValorUnitario { get; set; } = decimal.Zero;

        // Relacionamento
        public virtual ProdutoDto? Produto { get; set; }
        public virtual PedidoDto? Pedido { get; set; }
    }

}
