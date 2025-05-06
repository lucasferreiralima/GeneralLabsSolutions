using System.ComponentModel.DataAnnotations;
using GeneralLabSolutions.Domain.Enums;

namespace GeneralLabSolutions.WebAPI.DTOs
{
    public class TelefoneFornecedorDto
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [MaxLength(2, ErrorMessage = "O máximo de caracteres para o campo {0} é de {1} caracteres.")]
        public string DDD { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [MaxLength(9, ErrorMessage = "O máximo de caracteres para o campo {0} é de {1} caracteres.")]
        [Display(Name = "Número")]
        public string Numero { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Display(Name = "Tipo de Telefone")]
        public TipoDeTelefone TipoDeTelefone { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Display(Name = "Fornecedor Id")]
        public Guid FornecedorId { get; set; }
    }
}
