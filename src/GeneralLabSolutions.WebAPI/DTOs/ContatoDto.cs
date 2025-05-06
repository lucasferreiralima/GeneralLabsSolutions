using System.ComponentModel.DataAnnotations;
using GeneralLabSolutions.Domain.Enums;

namespace GeneralLabSolutions.WebAPI.DTOs
{
    public class ContatoDto
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Display(Name = "Nome do Contato")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Display(Name = "Email Principal")]
        [DataType(DataType.EmailAddress, ErrorMessage = "O campo {0} está inválido!")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Display(Name = "Telefone Principal")]
        public string Telefone { get; set; } = string.Empty;

        [Display(Name = "Email Alternativo")]
        public string EmailAlternativo { get; set; } = string.Empty;

        [Display(Name = "Telefone Alternativo")]
        public string TelefoneAlternativo { get; set; } = string.Empty;

        [Display(Name = "Observação")]
        public string Observacao { get; set; } = string.Empty;

        [Display(Name = "Tipo de Contato")]
        public TipoDeContato TipoDeContato { get; set; } = TipoDeContato.Comercial;
    }
}
