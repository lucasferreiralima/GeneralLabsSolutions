using System.ComponentModel.DataAnnotations;

namespace GeneralLabSolutions.WebAPI.DTOs
{
    public class CategoriaProdutoDto
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "A descrição é obrigatória.")]
        [StringLength(200, ErrorMessage = "A descrição deve ter no máximo 200 caracteres.")]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; } = string.Empty;

        public virtual ICollection<ProdutoDto>? Produtos { get; set; } = new List<ProdutoDto>();
    }
}