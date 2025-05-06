using FluentValidation;
using GeneralLabSolutions.Domain.Entities;
using GeneralLabSolutions.Domain.Entities.Base;
using GeneralLabSolutions.Domain.Enums;

namespace GeneralLabSolutions.Domain.Validations
{
    public class ClienteValidation : AbstractValidator<Cliente>
    {
        public ClienteValidation()
        {

            RuleFor(x => x.TipoDeCliente)
                .NotNull().WithMessage("O campo '{PropertyName}' não pode ser Nulo");

            RuleFor(x => x.Nome)
                .NotNull().WithMessage("O campo '{PropertyName}' não pode ser Nulo");

            RuleFor(x => x.Email)
                .NotNull().WithMessage("O campo '{PropertyName}' não pode ser Nulo");

            When(f => Enum.TryParse(typeof(TipoDePessoa), f.TipoDePessoa.ToString(), out var tipo) && (TipoDePessoa)tipo == TipoDePessoa.Fisica, () =>
            {
                RuleFor(f => f.Documento.Length).Equal(CpfValidacao.TamanhoCpf)
                    .WithMessage("O campo Documento precisa ter {ComparisonValue} caracteres e foi fornecido {PropertyValue}.");
                RuleFor(f => CpfValidacao.Validar(f.Documento)).Equal(true)
                    .WithMessage("O documento fornecido é inválido.");
            });



            When(f => Enum.TryParse(typeof(TipoDePessoa), f.TipoDePessoa.ToString(), out var tipo) && (TipoDePessoa)tipo == TipoDePessoa.Juridica, () =>
            {
                RuleFor(f => f.Documento.Length).Equal(CnpjValidacao.TamanhoCnpj)
                    .WithMessage("O campo Documento precisa ter {ComparisonValue} caracteres e foi fornecido {PropertyValue}.");
                RuleFor(f => CnpjValidacao.Validar(f.Documento)).Equal(true)
                    .WithMessage("O documento fornecido é inválido.");
            });


        }

    }
}
