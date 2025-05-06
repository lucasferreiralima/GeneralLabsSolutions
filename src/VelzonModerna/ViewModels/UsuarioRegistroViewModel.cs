using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using VelzonModerna.Extensions;

namespace VelzonModerna.ViewModels
{
    public class UsuarioRegistroViewModel
    {

        //[Required(ErrorMessage = "O campo {0} é obrigatório")]
        //[DisplayName("Nome Completo")]
        //public string Nome { get; set; }

        //[Required(ErrorMessage = "O campo {0} é obrigatório")]
        //[DisplayName("CPF")]
        //[Cpf]
        //public string Cpf { get; set; }

        //[Required(ErrorMessage = "O campo {0} é obrigatório")]
        //[EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
        //[DisplayName("E-mail")]
        //public string Email { get; set; }

        //[Required(ErrorMessage = "O campo {0} é obrigatório")]
        //[StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
        //public string Senha { get; set; }

        //[DisplayName("Confirme sua senha")]
        //[Compare("Senha", ErrorMessage = "As senhas não conferem.")]
        //public string SenhaConfirmacao { get; set; }
        [PersonalData]
        [Required(ErrorMessage = "O campo {0} é obrigatório!")]
        [StringLength(maximumLength: 35, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres!", MinimumLength = 2)]
        public string Apelido { get; set; }

        [PersonalData]
        [Display(Name = "Nome Completo")]
        [Required(ErrorMessage = "O campo {0} é obrigatório!")]
        [StringLength(maximumLength: 80, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres!", MinimumLength = 2)]
        public string NomeCompleto { get; set; }

        [PersonalData]
        [Required(ErrorMessage = "O campo {0} é obrigatório!")]
        [DataType(DataType.Date)]
        [Display(Name = "Data de Nascimento")]
        public DateTime DataNascimento { get; set; }

        [ProtectedPersonalData]
        [DataType(DataType.Text)]
        [StringLength(maximumLength: 255, ErrorMessage = "O campo {0} deve ter entre {2} e{1} caracteres", MinimumLength = 21)]
        public string ImgProfilePath { get; set; } = "imagemPadrao.png";

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
        public string Senha { get; set; } = string.Empty;

        [Compare("Senha", ErrorMessage = "As senhas não conferem.")]
        public string SenhaConfirmacao { get; set; } = string.Empty;
    }
}
