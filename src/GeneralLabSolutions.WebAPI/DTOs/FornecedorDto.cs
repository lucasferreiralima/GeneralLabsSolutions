using System.ComponentModel.DataAnnotations;
using GeneralLabSolutions.Domain.Enums;

namespace GeneralLabSolutions.WebAPI.DTOs
{
    public class FornecedorDto
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(200, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres.", MinimumLength = 2)]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(14, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres.", MinimumLength = 11)]
        public string? Documento { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Display(Name = "Status do Fornecedor")]
        public StatusDoFornecedor StatusDoFornecedor { get; set; } = StatusDoFornecedor.Ativo;

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Display(Name = "Pessoa Física / Jurídica")]

        public TipoDePessoa TipoDePessoa { get; set; }

        public virtual ICollection<TelefoneFornecedorDto> TelefonesFornecedores { get; set; }
            = new List<TelefoneFornecedorDto>();

        // Relatiolship
        public virtual ICollection<ProdutoDto> Produtos { get; set; }
            = new List<ProdutoDto>();
    }
}
