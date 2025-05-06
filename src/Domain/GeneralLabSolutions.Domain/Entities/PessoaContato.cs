using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneralLabSolutions.Domain.Entities.Base;

namespace GeneralLabSolutions.Domain.Entities
{
    public class PessoaContato : EntityBase
    {
        public Guid PessoaId { get; set; }
        public Pessoa Pessoa { get; set; }

        public Guid ContatoId { get; set; }
        public Contato Contato { get; set; }
    }
}
