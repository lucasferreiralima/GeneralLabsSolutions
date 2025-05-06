using System.ComponentModel.DataAnnotations;

namespace GeneralLabSolutions.Identidade.Dtos
{
    /// <summary>
    /// DTO para transporte de dados para renovar token
    /// </summary>
    public class RenovarTokenDto
    {
        [Required(ErrorMessage = "O RefreshToken é obrigatório")]
        public string RefreshToken { get; set; }
    }
}
