using GeneralLabSolutions.Domain.Entities;

namespace GeneralLabSolutions.Domain.Services.Abstractions
{
    public class MinhasAbstracoes{}

    public interface ICategoriaDomainService
    {
        public Task AddCategoriaAsync(CategoriaProduto model);
        public Task UpdateCategoriaAsync(CategoriaProduto model);
        public Task DeleteCategoriaProdutoAsync(CategoriaProduto model);

    }

    public interface IClienteDomainService
    {
        Task<bool> ValidarAddCliente(Cliente model);
        Task<bool> ValidarUpdCliente(Cliente model);
        Task<bool> ValidarDelCliente(Cliente model);


        Task AddClienteAsync(Cliente model);
        Task UpdateClienteAsync(Cliente model);
        Task DeleteClienteAsync(Cliente model);
    }

    public interface IKanbanTaskDomainService
    {
        Task<bool> ValidarAddKanbanTaskAsync(KanbanTask model);
        Task AddkanbanTaskAsync(KanbanTask model);
    }

    public interface IParticipanteDomainService
    {
        Task<bool> ValidarAddParticipanteAsync(Participante model);
        Task AddParticipanteAsync(Participante model);

    }

}
