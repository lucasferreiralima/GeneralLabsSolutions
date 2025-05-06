using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;
using MediatR;

namespace GeneralLabSolutions.Domain.Mensageria
{
    public abstract class Command : Message, IRequest<ValidationResult>
    {
        public DateTime Timestamp { get; private set; }
        public ValidationResult ValidationResult { get; set; }

        protected Command()
        {
            Timestamp = DateTime.Now;
        }

        /// <summary>
        /// Para obrigar as classes que herdam de Command
        /// a implementar com override o método EhValido()
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual bool EhValido()
        {
            throw new NotImplementedException();
        }
    }
}