using GeneralLabSolutions.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace VelzonModerna.Controllers
{
    public class ConsolidadoFornecedorController : Controller
    {
        private readonly IConsolidadoFornecedorRepository _consolidadoFornecedorRepository;

        public ConsolidadoFornecedorController(IConsolidadoFornecedorRepository consolidadoFornecedorRepository)
        {
            _consolidadoFornecedorRepository = consolidadoFornecedorRepository;
        }

        [HttpGet]
        public async Task<IActionResult> SelecionarFornecedor()
        {
            try
            {
                var fornecedores = await _consolidadoFornecedorRepository.ObterTodosFornecedoresAsync();
                return View(fornecedores);
            } catch (Exception ex)
            {
                // Logar a exceção
                Console.WriteLine(ex);
                return StatusCode(500, "Erro interno no servidor.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObterConsolidado(Guid fornecedorId)
        {
            try
            {
                var consolidadoFornecedor = await _consolidadoFornecedorRepository.ObterConsolidadoFornecedorPorIdAsync(fornecedorId);

                if (consolidadoFornecedor == null)
                {
                    return PartialView("_FornecedorSemResumoPartial");
                }

                return View("ConsolidadoResult", consolidadoFornecedor);
            } catch (Exception ex)
            {
                // Logar a exceção
                Console.WriteLine(ex);
                return StatusCode(500, "Erro interno no servidor.");
            }
        }
    }
}