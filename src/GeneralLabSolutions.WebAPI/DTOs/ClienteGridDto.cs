using System.ComponentModel.DataAnnotations;
using GeneralLabSolutions.Domain.Enums;

namespace GeneralLabSolutions.WebAPI.DTOs
{
    public class ClienteGridDto
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório!")]
        [StringLength(200, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres!", MinimumLength = 2)]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo {0} é obrigatório!")]
        [StringLength(254, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres!", MinimumLength = 6)]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email inválido!")]
        public string Email { get; set; } = string.Empty;


        [Required(ErrorMessage = "O campo {0} é obrigatório!")]
        [StringLength(14, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres!", MinimumLength = 11)]
        public string Documento { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo {0} é obrigatório!")]
        [Display(Name = "Status do Cliente")]
        public StatusDoCliente StatusDoCliente { get; set; }
            = StatusDoCliente.Ativo;

        [Required(ErrorMessage = "O campo {0} é obrigatório!")]
        [Display(Name = "Tipo de Cliente")]
        public TipoDeCliente TipoDeCliente { get; set; }
            = TipoDeCliente.Comum;

        [Required(ErrorMessage = "O campo {0} é obrigatório!")]
        [Display(Name = "Tipo de Pessoa")]
        public TipoDePessoa TipoDePessoa { get; set; }
            = TipoDePessoa.Juridica;

    }
}
