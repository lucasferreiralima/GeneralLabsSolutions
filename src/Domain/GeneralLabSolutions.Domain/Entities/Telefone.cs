using GeneralLabSolutions.Domain.Entities.Base;
using GeneralLabSolutions.Domain.Enums;

namespace GeneralLabSolutions.Domain.Entities
{
    public class Telefone : EntityBase
    {
        // EF
        public Telefone() { }

        public Telefone(string ddd, string numero, TipoDeTelefone tipoDeTelefone)
        {
            DDD = ddd;
            Numero = numero;
            TipoDeTelefone = tipoDeTelefone;
        }

        // Código de área do telefone
        public string DDD { get; set; }
        // Número do telefone
        public string Numero { get; set; }
        // Tipo de telefone (e.g., Celular, Residencial)
        public TipoDeTelefone TipoDeTelefone { get; set; }
        // Identificador da Pessoa associada a este telefone

        public virtual ICollection<PessoaTelefone> PessoasTelefones { get; set; } = new List<PessoaTelefone>();
    }

}
