using FluentValidation;
using GeneralLabSolutions.Domain.Entities;
using GeneralLabSolutions.Domain.Entities.Base;
using GeneralLabSolutions.Domain.Enums;
using GeneralLabSolutions.Domain.Mensageria;

namespace GeneralLabSolutions.WebAPI.Application.Commands
{
    public class RegistrarClienteCommand : Command
    {
        public RegistrarClienteCommand(Guid id, string nome, string documento, TipoDePessoa tipoDePessoa, TipoDeCliente tipoDeCliente, StatusDoCliente statusDoCliente, string email)
        {
            Id = id;
            AggregateId = id;
            Nome = nome;
            Documento = documento;
            TipoDePessoa = tipoDePessoa;
            Email = email;
        }

        public Guid Id { get; set; }

        public string Nome { get; set; }

        public string Documento { get; set; }
        public TipoDePessoa TipoDePessoa { get; set; }
        public TipoDeCliente TipoDeCliente { get; set; }
        public StatusDoCliente StatusDoCliente { get; set; }

        // Email do cliente
        public string Email { get; set; }
        // Status do cliente (e.g., Ativo, Inativo)

        public override bool EhValido()
        {
            ValidationResult = new AddClienteValidation().Validate(this);
            return ValidationResult.IsValid;
        }

    }


    public class AddClienteValidation : AbstractValidator<RegistrarClienteCommand>
    {
        public AddClienteValidation()
        {

            RuleFor(x => x.TipoDeCliente)
                .NotNull().WithMessage("O campo '{PropertyName}' não pode ser Nulo");

            RuleFor(x => x.TipoDePessoa)
                .NotNull().WithMessage("O campo '{PropertyName}' não pode ser Nulo");

            RuleFor(x => x.StatusDoCliente)
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