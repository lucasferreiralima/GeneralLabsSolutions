using GeneralLabSolutions.Domain.Entities;
using GeneralLabSolutions.Domain.Enums;
using GeneralLabSolutions.Domain.Mensageria;

namespace GeneralLabSolutions.WebAPI.Application.Events
{
    public class ClienteRegistradoEvent : DomainEvent
    {
        public ClienteRegistradoEvent(Guid aggregateId, Guid id, string nome, string documento, TipoDePessoa tipoDePessoa, string email): base(aggregateId)
        {
            AggregateId = aggregateId;
            Id = id;
            Email = email;
            Nome = nome;
            Documento = documento;
            TipoDePessoa = tipoDePessoa;
        }

        public Guid Id { get; set; }

        public string Nome { get; set; }

        public string Documento { get; set; }
        public string Email { get; set; }
        public TipoDePessoa TipoDePessoa { get; set; }
    }
}
