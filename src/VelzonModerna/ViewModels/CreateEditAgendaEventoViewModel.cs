using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace VelzonModerna.ViewModels;

public class CreateEditAgendaEventoViewModel
{
    [JsonPropertyName("id")]
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    public Guid Id { get; set; }

    [JsonPropertyName("participanteId")]
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    public Guid ParticipanteId { get; set; }

    [JsonPropertyName("title")]
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    public string Title { get; set; }

    [JsonPropertyName("description")]
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    public string Description { get; set; }

    [JsonPropertyName("start")]
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    public DateTime Start { get; set; }

    [JsonPropertyName("end")]
    public DateTime? End { get; set; }

    [JsonPropertyName("color")]
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    public string Color { get; set; }

    [JsonPropertyName("allDay")]
    public bool AllDay { get; set; } = false;

    [JsonPropertyName("participantes")]
    public List<SelectListItem> Participantes { get; set; }

    [JsonPropertyName("nomeParticipante")]
    public string? NomeParticipante { get; set; }


    public CreateEditAgendaEventoViewModel()
    {
        Participantes = new List<SelectListItem>();
    }
}
