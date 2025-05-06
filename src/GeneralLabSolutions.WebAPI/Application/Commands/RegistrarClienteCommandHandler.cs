using FluentValidation.Results;
using GeneralLabSolutions.Domain.Entities;
using GeneralLabSolutions.Domain.Enums;
using GeneralLabSolutions.Domain.Interfaces;
using GeneralLabSolutions.WebAPI.Application.Events;
using FluentValidation;
using MediatR;
using GeneralLabSolutions.Domain.Mensageria;

namespace GeneralLabSolutions.WebAPI.Application.Commands
{
    public class RegistrarClienteCommandHandler : CommandHandler,
        IRequestHandler<RegistrarClienteCommand, ValidationResult>
    {
        private readonly IClienteRepository _clienteRepository;

        private readonly IQueryGenericRepository<Cliente, Guid> _query;

        public RegistrarClienteCommandHandler(IClienteRepository clienteRepository, IQueryGenericRepository<Cliente, Guid> query)
        {
            _clienteRepository = clienteRepository;
            _query = query;
        }


        public async Task<ValidationResult> Handle(RegistrarClienteCommand message, CancellationToken cancellationToken)
        {
            if (!message.EhValido())
                return message.ValidationResult;

            var cliente = new Cliente(message.Nome, message.Documento, message.TipoDePessoa, message.Email);



            // Todo: Verificar se podemos otimizar, pois não há necessidade de três consultas na base já que a primeira regra de negóio já foi quebrada
            if (!await ClienteValido(cliente)) return ValidationResult;

            // Adicionar o Cliente no stack de gravação
            await _clienteRepository.AddAsync(cliente);

            // Gerar o evento de Cliente Adicionado
            cliente.AdicionarEvento(new ClienteRegistradoEvent(message.AggregateId, message.Id, message.Nome, message.Documento, message.TipoDePessoa, message.Email));

            // Persistir na base de dados
            return await PersistirDados(_clienteRepository.UnitOfWork);
        }

        public async Task<bool> ClienteValido(Cliente cliente)
        {
            bool isValid = true;

            // Busca por Cpf ou Cnpj
            if (_query.SearchAsync(c => c.Documento == cliente.Documento).Result.Any())
            {
                AdicionarErro("Já existe um Cliente com este documento informado.");
                isValid = false;
            }


            // Cliente novo com Email existente != em Update
            if (_query.SearchAsync(c => c.Email == cliente.Email).Result.Any())
            {
                AdicionarErro("Já existe um Cliente com este Email. Tente outro!");
                isValid = false;
            }

            if (cliente.StatusDoCliente == StatusDoCliente.Inativo)
            {
                AdicionarErro("Nenhum cliente pode ser Adicionado com o Status de 'Inativo'.");
                isValid = false;
            }

            if (cliente.TipoDeCliente == TipoDeCliente.Inadimplente)
            {
                AdicionarErro("Não se pode adicionar um cliente como 'Inadimplente'.");
                isValid = false;
            }

            if (await _query.GetByIdAsync(cliente.Id) is not null)
            {
                AdicionarErro("Já existe um Cliente cadastrado com este ID.");
                isValid = false;
            }

            return isValid;
        }
    }
}