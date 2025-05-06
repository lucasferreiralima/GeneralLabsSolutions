using GeneralLabSolutions.Domain.Enums;

namespace GeneralLabSolutions.WebAPI.DTOs
{
    public class VendedorGridDto
    {
        public Guid Id { get; set; }
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string? Documento { get; set; }
        public TipoDePessoa TipoDePessoa { get; set; }
        = TipoDePessoa.Juridica;
    }
}
