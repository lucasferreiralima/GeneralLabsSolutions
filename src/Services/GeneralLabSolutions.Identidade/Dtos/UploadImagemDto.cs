using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace GeneralLabSolutions.Identidade.Dtos
{
    /// <summary>
    /// DTO para transportar os dados para upload de imagem de perfil
    /// </summary>
    public class UploadImagemDto
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string UserId { get; set; } = string.Empty; // ID do usuário

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public IFormFile Imagem { get; set; } = null!;
    }
}