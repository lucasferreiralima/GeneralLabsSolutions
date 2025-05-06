using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;
using GeneralLabSolutions.Domain.Interfaces;

namespace GeneralLabSolutions.Domain.Mensageria
{
    public abstract class CommandHandler
    {
        protected ValidationResult ValidationResult;

        protected CommandHandler()
        {
            ValidationResult = new ValidationResult();
        }
        protected void AdicionarErro(string mensagem)
        {
            ValidationResult.Errors.Add(new ValidationFailure(string.Empty, mensagem));
        }
        protected async Task<ValidationResult> PersistirDados(IUnitOfWork uow)
        {
            if (!await uow.CommitAsync())
                AdicionarErro("Houve um erro ao persistir os dados");

            return ValidationResult;
        }
    }
}
