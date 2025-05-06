using System.ComponentModel;

namespace GeneralLabSolutions.Domain.Enums
{
    public class MeusEnums
    {
    }

    public enum Sexo
    {
        [Description("Feminino")] Feminino, // Peso: 48
        [Description("Masculino")] Masculino, // Peso: 48
        [Description("Outro")] Outro // Peso: 4

    }
    public enum StatusDoCliente
    {
        [Description("Ativo")] Ativo, // Peso: 30
        [Description("Inativo")] Inativo // Peso 5
    }
    public enum StatusDoFornecedor
    {
        Ativo, // Peso: 30
        Inativo // Peso: 5
    }
    public enum StatusDoPedido
    {
        [Description("Orçamento do Vendedor")] Orcamento, // Peso: 10 - maior que cancelado
        [Description("Em Processamento")] EmProcessamento, // Peso: 20 - ainda maior
        [Description("Pago")] Pago, // Peso: 30 - mais alto (prioritário)
        [Description("Enviado")] Enviado, // Peso: 10 - médio
        [Description("Entregue")] Entregue, // Peso: 20 - grande
        [Description("Cancelado")] Cancelado // Peso: 5 - mínimo (raridade)
    }
    public enum StatusDoProduto
    {
        [Description("Dropshipping")] Dropshipping, // Peso: 30 => Trocado por Dropshipping
        [Description("Inativo")] Inativo, // Peso: 5
        [Description("Reservado")] Reservado, // 20
        [Description("Em Estoque")] EmEstoque, // 10
        [Description("Esgotado")] Esgotado // Todo: Peso 5
    }
    public enum StatusDoVendedor
    {
        Admin, // Peso: 15
        Contratado, // Peso: 10
        FreeLance, // Peso: 20
        Inativo // Peso: 5
    }
    public enum TipoDeCliente
    {
        [Description("Especial")] Especial, // Peso 9
        [Description("Comum")] Comum, // Peso 32
        [Description("Inadimplente")] Inadimplente // Peso 4
    }
    public enum TipoDeContato
    {
        Inativo, // Peso: 5
        Comercial, // Peso: 35
        Pessoal, // Peso: 10
        ProspeccaoCliente, // Peso: 25
        ProspeccaoVendedor, // Peso: 10
        ProspeccaoFornecedor // Peso: 15
    }
    public enum TipoDePessoa
    {
        [Description("Física")] Fisica, // Peso: 5
        [Description("Jurídica")] Juridica // Peso: 25
    }
    public enum TipoDeTelefone
    {
        Celular, // Peso: 10
        Residencial, // Peso: 5
        Comercial, // Peso: 15
        Recado, // Peso: 5
        Outro // Peso: 3
    }

    // TipoDescontoVoucher
    public enum TipoDescontoVoucher
    {
        Porcentagem = 0,
        Valor = 1
    }

}
