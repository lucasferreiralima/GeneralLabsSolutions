using System.ComponentModel.DataAnnotations;

namespace GeneralLabSolutions.Domain.Enums
{
    public enum TipoEvento
    {
        [Display(Name = "bg-danger-subtle")] Perigo,
        [Display(Name = "bg-success-subtle")] Sucesso,
        [Display(Name = "bg-primary-subtle")] Primario,
        [Display(Name = "bg-info-subtle")] Info,
        [Display(Name = "bg-dark-subtle")] Escuro,
        [Display(Name = "bg-warning-subtle")] Alerta
    }
}