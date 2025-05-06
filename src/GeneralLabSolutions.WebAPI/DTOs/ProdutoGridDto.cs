using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using GeneralLabSolutions.Domain.Enums;

namespace GeneralLabSolutions.WebAPI.DTOs
{
    public class ProdutoGridDto
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório!")]
        [StringLength(20, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres!", MinimumLength = 2)]
        [Display(Name = "Código do Produto")]
        public string? Codigo { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório!")]
        [StringLength(600, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres!", MinimumLength = 2)]
        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }


        public string? Ncm { get; set; }


        [DataType(DataType.Currency, ErrorMessage = "O campo {0} está inválido!")]
        [DisplayName("Valor Unitário")]
        public decimal ValorUnitario { get; set; }

        [Display(Name = "Status do Produto")]
        [Required(ErrorMessage = "O campo {0} é obrigatório!")]
        public StatusDoProduto StatusDoProduto { get; set; }
            = StatusDoProduto.Dropshipping;

        [DataType(DataType.DateTime, ErrorMessage = "O campo {0} é obrigatório!")]
        public DateTime DataDeValidade { get; set; }

    }
}
