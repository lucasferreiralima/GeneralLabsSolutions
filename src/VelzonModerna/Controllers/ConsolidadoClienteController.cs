using GeneralLabSolutions.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

namespace VelzonModerna.Controllers
{
    public class ConsolidadoClienteController : Controller
    {
        private readonly IConsolidadoClienteRepository _consolidadoClienteRepository;

        public ConsolidadoClienteController(IConsolidadoClienteRepository consolidadoClienteRepository)
        {
            _consolidadoClienteRepository = consolidadoClienteRepository;
        }

        // Método GET para mostrar a view onde o cliente será selecionado
        [HttpGet]
        public async Task<IActionResult> SelecionarCliente()
        {
            try
            {
                // Obter todos os clientes do repositório
                var clientes = await _consolidadoClienteRepository.GetAllClientesAsync(); // Isso assume que você tem um método para buscar todos os clientes
                ModelState.Clear();

                return View(clientes); // Passa a lista de clientes para a view
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Erro no servidor ao carregar a lista de clientes.");
            }
        }



        [HttpGet]
        public async Task<IActionResult> ObterConsolidado(Guid clienteId)
        {
            try
            {
                var consolidadoCliente = await _consolidadoClienteRepository.ObterConsolidadoClientePorIdAsync(clienteId);

                if (consolidadoCliente == null || consolidadoCliente.QuantidadeDePedidos == 0)
                {
                    // Adiciona o erro no ModelState
                    ModelState.AddModelError("ClienteSemPedidos", "Este cliente não possui pedidos.");

                    // Obtenha novamente a lista de clientes e retorne para a view "SelecionarCliente"
                    var clientes = await _consolidadoClienteRepository.GetAllClientesAsync();
                    return View("SelecionarCliente", clientes); // Retorna a lista de clientes
                }

                return View("ConsolidadoResult", consolidadoCliente); // Retorna o consolidado se houver pedidos
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Erro no servidor ao carregar os dados consolidados.");
            }
        }




        [HttpGet]
        public async Task<IActionResult> ObterItensPedido(Guid pedidoId)
        {
            var itensPedido = await _consolidadoClienteRepository.ObterItensPedido(pedidoId);
            if (itensPedido.Itens.Any())
            {
                return PartialView("_ItensPedidoPartial", itensPedido);
            }
            return NotFound();
        }



    }
}
