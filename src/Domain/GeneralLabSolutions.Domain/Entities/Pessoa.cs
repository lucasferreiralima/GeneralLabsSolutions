using GeneralLabSolutions.Domain.Entities.Base;
using GeneralLabSolutions.Domain.Enums;

namespace GeneralLabSolutions.Domain.Entities
{
    public class Pessoa : EntityBase
    {
        /// <summary>
        /// Construtor vazio para uso pelo EF
        /// </summary>
        public Pessoa() { }


        public virtual ICollection<PessoaTelefone> PessoasTelefones { get; set; } = new List<PessoaTelefone>();
        public virtual ICollection<PessoaContato> PessoasContatos { get; set; } = new List<PessoaContato>();

    }
}
