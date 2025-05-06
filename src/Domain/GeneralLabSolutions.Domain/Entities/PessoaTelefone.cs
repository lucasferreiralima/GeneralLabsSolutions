using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneralLabSolutions.Domain.Entities.Base;

namespace GeneralLabSolutions.Domain.Entities
{
    /// <summary>
    /// Modelo que associa Pessoa com Telefone
    /// </summary>
    public class PessoaTelefone
    {
        // ToDo: Retirei a herança de EntityBase, pois a chave primária é a composta: PessoaId + TelefoneId; Documentador: Não perca de vista esta anotação!

        public Guid PessoaId { get; set; }
        public Pessoa Pessoa { get; set; }

        public Guid TelefoneId { get; set; }
        public Telefone Telefone { get; set; }
    }

}
