using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using GeneralLabSolutions.Domain.Enums;

namespace GeneralLabSolutions.WebAPI.DTOs
{
    public class ProdutoDto
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


        public string? Imagem { get; set; }

        [DisplayName("Imagem do Produto")]
        public IFormFile? ImagemUpload { get; set; }


        // Relacionamentos
        [Display(Name = "Id da Categoria")]
        public Guid CategoriaId { get; set; }


        public virtual CategoriaProdutoDto? CategoriaProduto { get; set; }

        [Display(Name = "Id do Fornecedor")]
        public Guid FornecedorId { get; set; }

        public virtual FornecedorDto? Fornecedor { get; set; }

    }
}
