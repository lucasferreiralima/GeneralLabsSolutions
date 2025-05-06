using GeneralLabSolutions.Domain.Entities.Base;
using GeneralLabSolutions.Domain.Enums;

namespace GeneralLabSolutions.Domain.Entities
{
    public class Contato : EntityBase
    {
        // EF Core
        public Contato() { }

        public Contato(string nome, string email, string telefone)
        {
            Nome = nome;
            Email = email;
            Telefone = telefone;
        }

        public string Nome { get; private set; }
        public string Email { get; private set; }
        public string Telefone { get; private set; }
        public string EmailAlternativo { get; set; } = string.Empty;
        public string TelefoneAlternativo { get; set; } = string.Empty;
        public string Observacao { get; set; } = string.Empty;
        public TipoDeContato TipoDeContato { get; set; } = TipoDeContato.Comercial;

        public virtual ICollection<PessoaContato> PessoasContatos { get; set; } = new List<PessoaContato>();


    }
}