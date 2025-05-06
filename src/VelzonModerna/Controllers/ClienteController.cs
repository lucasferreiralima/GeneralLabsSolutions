using AutoMapper;
using GeneralLabSolutions.Domain.Entities;
using GeneralLabSolutions.Domain.Interfaces;
using GeneralLabSolutions.Domain.Notigfications;
using GeneralLabSolutions.Domain.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using VelzonModerna.Controllers.Base;
using VelzonModerna.ViewModels;

namespace VelzonModerna.Controllers
{
    public class ClienteController : BaseMvcController
    {

        private readonly IMapper _mapper;

        private readonly IClienteRepository _clienteRepository;
        private readonly IClienteDomainService _clienteDomainService;
        private readonly IQueryGenericRepository<Cliente, Guid> _query;

        public ClienteController(INotificador notificador,
                                 IMapper mapper,
                                 IClienteRepository clienteRepository,
                                 IClienteDomainService clienteDomainService,
                                 IQueryGenericRepository<Cliente, Guid> query) : base(notificador)
        {
            _mapper = mapper;
            _clienteRepository = clienteRepository;
            _clienteDomainService = clienteDomainService;
            _query = query;
        }

        // GET: Cliente
        public async Task<IActionResult> Index()
        {
            var listClienteViewModel
                = _mapper.Map<IEnumerable<ClienteViewModel>>(await _query.GetAllAsync());

            return View(listClienteViewModel);
        }

        // GET: Cliente/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var clienteViewModel
                = _mapper.Map<ClienteViewModel>(await _query.GetByIdAsync(id));

            if (clienteViewModel == null)
            {
                return NotFound();
            }

            return View(clienteViewModel);
        }


        // GET: Cliente/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClienteViewModel clienteViewModel)
        {
            if (!ModelState.IsValid)
            {
                // Renderizar o ViewComponent manualmente em caso de erro
                return View(clienteViewModel);
            }

            var cliente = _mapper.Map<Cliente>(clienteViewModel);
            await _clienteDomainService.AddClienteAsync(cliente);

            // Mostra notificações de Validação de regra de negócio,
            // caso haja e NÃO deixa que o CommitAsync seja chamao.
            if (!OperacaoValida()) return View(clienteViewModel);            

            await _clienteRepository.UnitOfWork.CommitAsync();
            TempData ["Sucesso"] = "Cliente Adicionado com Sucesso!";

            return RedirectToAction(nameof(Index)); 
        }



        // GET: Cliente/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            var clienteViewModel = _mapper.Map<ClienteViewModel>(await _query.GetByIdAsync(id));

            if (clienteViewModel == null)
            {
                return NotFound();
            }

            // Garantir que o Nome não seja null
            clienteViewModel.Nome = clienteViewModel.Nome ?? string.Empty;

            return View(clienteViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ClienteViewModel clienteViewModel)
        {
            if (id != clienteViewModel.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid) return View(clienteViewModel);

            await _clienteDomainService.UpdateClienteAsync(_mapper.Map<Cliente>(clienteViewModel));

            // Validação de regra de negócio
            if (!OperacaoValida()) return View(clienteViewModel);

            await _clienteRepository.UnitOfWork.CommitAsync();

            TempData ["Sucesso"] = "Cliente Atualizado com Sucesso!";
            return RedirectToAction(nameof(Index));

        }


        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var clienteViewModel = _mapper.Map<ClienteViewModel>(await _query.GetByIdAsync(id));


            if (clienteViewModel == null)
            {
                return NotFound("Recurso não encontrado!");
            }

            return View(clienteViewModel);
        }

        // POST: Cliente/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var clienteViewModel = _mapper.Map<ClienteViewModel>(await _query.GetByIdAsync(id));

            if (clienteViewModel != null)
            {
                await _clienteDomainService.DeleteClienteAsync(_mapper.Map<Cliente>(clienteViewModel));

                // Validação de regra de negócio
                if (!OperacaoValida()) return View(clienteViewModel);


                await _clienteRepository.UnitOfWork.CommitAsync();
                TempData ["Sucesso"] = "Cliente excluido com sucesso!";
                return RedirectToAction(nameof(Index));
            }

            return View(clienteViewModel);


        }

        private async Task<bool> ClienteExists(Guid id)
        {
            return await _clienteRepository.TemCliente(id);
        }
    }
}
