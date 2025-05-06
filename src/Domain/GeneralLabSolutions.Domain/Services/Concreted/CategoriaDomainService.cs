using GeneralLabSolutions.Domain.Entities;
using GeneralLabSolutions.Domain.Interfaces;
using GeneralLabSolutions.Domain.Notigfications;
using GeneralLabSolutions.Domain.Services.Abstractions;

namespace GeneralLabSolutions.Domain.Services.Concreted;

public class CategoriaDomainService : BaseService, ICategoriaDomainService
{

    private readonly ICategoriaRepository _categoriaRepository;
    private readonly IQueryGenericRepository<CategoriaProduto, Guid> _query;

    public CategoriaDomainService(INotificador notificador, ICategoriaRepository categoriaRepository, IQueryGenericRepository<CategoriaProduto, Guid> query) : base(notificador)
    {
        _categoriaRepository = categoriaRepository;
        _query = query;
    }

    public async Task AddCategoriaAsync(CategoriaProduto model)
    {
        await _categoriaRepository.AddAsync(model);
    }


    public async Task UpdateCategoriaAsync(CategoriaProduto model)
    {
        await _categoriaRepository.UpdateAsync(model);
    }

    public async Task DeleteCategoriaProdutoAsync(CategoriaProduto model)
    {
        //regras de negócio
        await _categoriaRepository.DeleteAsync(model);
    }

}