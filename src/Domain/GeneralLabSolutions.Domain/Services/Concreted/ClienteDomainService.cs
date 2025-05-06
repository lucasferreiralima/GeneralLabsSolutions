using System.Reflection;
using GeneralLabSolutions.Domain.Entities;
using GeneralLabSolutions.Domain.Enums;
using GeneralLabSolutions.Domain.Interfaces;
using GeneralLabSolutions.Domain.Notigfications;
using GeneralLabSolutions.Domain.Services.Abstractions;
using GeneralLabSolutions.Domain.Validations;

namespace GeneralLabSolutions.Domain.Services.Concreted
{
    public class ClienteDomainService : BaseService, IClienteDomainService
    {

        private readonly IClienteRepository _clienteRepository;
        private readonly IQueryGenericRepository<Cliente, Guid> _query;

        public ClienteDomainService(IClienteRepository clienteRepository,
                                    INotificador notificador,
                                    IQueryGenericRepository<Cliente, Guid> query) 
            : base(notificador)
        {
            _clienteRepository = clienteRepository;
            _query = query;
        }


        #region: Regras de Negócios Agrupadas para evitar várias interrupções: Add
        public async Task<bool> ValidarAddCliente(Cliente model)
        {
            bool isValid = true;

            if (_query.SearchAsync(c => c.Documento == model.Documento).Result.Any())
            {
                Notificar("Já existe um Cliente com este documento informado.");
                isValid = false;
            }

            // Cliente novo com Email existente != em Update
            if (_query.SearchAsync(c => c.Email == model.Email).Result.Any())
            {
                Notificar("Já existe um Cliente com este Email. Tente outro!");
                isValid = false;
            }


            if (model.StatusDoCliente == StatusDoCliente.Inativo)
            {
                Notificar("Nenhum cliente pode ser Adicionado com o Status de 'Inativo'.");
                isValid = false;
            }

            if (model.TipoDeCliente == TipoDeCliente.Inadimplente)
            {
                Notificar("Não se pode adicionar um cliente como 'Inadimplente'.");
                isValid = false;
            }

            var clienteExiste = await _query.GetByIdAsync(model.Id);
            if (clienteExiste is not null)
            {
                Notificar("Já existe um Cliente cadastrado com este ID.");
                isValid = false;
            }

            if (!ExecutarValidacao(new ClienteValidation(), model))
            {
                isValid = false;
            }

            return isValid;
        }
        #endregion

        #region: Regras de Negócios: Update
        public async Task<bool> ValidarUpdCliente(Cliente model)
        {
            bool isValid = true;

            // Verificar se o cliente está inadimplente
            // Todo: E se quisermos alterar o TipoDeCliente para 'Comum', assim que ele quitar a dívida?
            // Todo: E se ele está Inadimplente, quer dizer que o seu `StatusDoPedido` deve estar "Inativo" ?
            // Todo: A ideia é; ao alteramos o Tipo do Client para Inadimplente seu Status deve ser alterado para Inativo???
            // Todo: Estas observações acima devem ser levadas em conta ao executar esta validação... !!!?
            var clienteAtual = await _query.GetByIdAsync(model.Id);
            if (clienteAtual != null && clienteAtual.TipoDeCliente == TipoDeCliente.Inadimplente)
            {
                Notificar("Este cliente não pode ser atualizado, pois está inadimplente.");
                isValid = false;
            }


            // Verificar se o email já está em uso por outro cliente
            var clienteComMesmoEmail = await _query.SearchAsync(c => c.Email == model.Email && c.Id != model.Id);
            if (clienteComMesmoEmail.Any())
            {
                Notificar("Já existe um Cliente com este Email. Tente outro!");
                isValid = false;
            }

            // Verificar se o documento já está em uso por outro cliente
            var clienteComMesmoDocumento = await _query.SearchAsync(c => c.Documento == model.Documento && c.Id != model.Id);
            if (clienteComMesmoDocumento.Any())
            {
                Notificar("Já existe um Cliente com este documento informado.");
                isValid = false;
            }


            if (!ExecutarValidacao(new ClienteValidation(), model))
                isValid = false;


            return isValid;
        }
        #endregion


        #region: Regras de Negócios: Delete
        public async Task<bool> ValidarDelCliente(Cliente model)
        {
            bool isValid = true;

            // Todo: E se, ao invés de excluirmos apenas deixá-lo "Inativo"?
            // Todo: Cliente Inativo não pode Comprar... !!?
            var clienteComPedidos = await _query
                .SearchAsync(c => c.Id == model.Id && c.Pedidos.Any());

            if (clienteComPedidos.Any())
            {
                Notificar("O cliente possui pedidos e não pode ser excluído.");
                isValid = false;
            }

            if (!ExecutarValidacao(new DeleteClienteValidation(), model)) isValid = false;

            return isValid;
        }
        #endregion

        public async Task AddClienteAsync(Cliente model)
        {
            // Verifica as regras de negócio e validações
            if (!await ValidarAddCliente(model)) return;


            // Todo: Mover os Eventos para a Camada de Domain
            //model.AdicionarEvento(new AddClienteRegistrado)
                
            await _clienteRepository.AddAsync(model);

            // Todo: PersistirDados

        }

        public async Task UpdateClienteAsync(Cliente model)
        {
            // Verifica as regras de negócio e validações
            if (!await ValidarUpdCliente(model)) return;

            await _clienteRepository.UpdateAsync(model);
        }

        public async Task DeleteClienteAsync(Cliente model)
        {
            // Verifica as regras de negócio e validações
            if (!await ValidarDelCliente(model)) return;

            await _clienteRepository.DeleteAsync(model);
        }

    }
}
