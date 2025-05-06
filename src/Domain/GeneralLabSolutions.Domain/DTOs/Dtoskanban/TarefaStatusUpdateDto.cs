namespace GeneralLabSolutions.Domain.DTOs.Dtoskanban
{
    public class TarefaStatusUpdateDto
    {
        public Guid Id { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
