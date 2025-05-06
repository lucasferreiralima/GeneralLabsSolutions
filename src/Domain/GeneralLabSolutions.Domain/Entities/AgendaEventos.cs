using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneralLabSolutions.Domain.Entities.Base;

namespace GeneralLabSolutions.Domain.Entities
{
    public class AgendaEventos : EntityBase
    {

        [Display(Name = "Título")]
        //[Column(name: "Titulo")]
        public string? Title { get; set; }


        [Display(Name = "Descrição")]
        //[Column(name: "Descricao")]
        public string? Description { get; set; }

        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public string? Color { get; set; }
        public Boolean AllDay { get; set; }

        public Guid ParticipanteId { get; set; }
        public virtual Participante? Participante { get; set; }

    }
}
