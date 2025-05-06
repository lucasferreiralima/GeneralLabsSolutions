using GeneralLabSolutions.Domain.Enums;
using GeneralLabSolutions.InfraStructure.Data;
using GeneralLabSolutions.WebAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GeneralLabSolutions.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidoController : ControllerBase
    {

        private readonly AppDbContext _context;

        public PedidoController(AppDbContext context)
        {
            _context = context;
        }


        /// <summary>
        /// Todo: AsSplitQuery, PagedResult, Filter / Search, Sort
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("pedidos")]
        [ProducesResponseType(typeof(IEnumerable<PedidoGridDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ListaDePedidos()
        {
            try
            {
                var listaPedidos = await _context.Pedido
                    .AsSplitQuery()
                    .AsNoTracking()
                    .Select(item => new PedidoGridDto
                    {
                        Id = item.Id,
                        ClienteId = item.ClienteId,
                        DataPedido = item.DataPedido,
                        StatusDoPedido = item.StatusDoPedido,
                        VendedorId = item.VendedorId
                    }).ToListAsync();

                if (!listaPedidos.Any())
                {
                    // Retorna 404 se nenhum pedido for encontrado
                    return NotFound("Nenhum Pedido Encontrado.");
                }

                // Retorna 200 com a lista de pedidos
                return Ok(listaPedidos);
            } catch (Exception ex)
            {
                // Log do erro aqui
                return StatusCode(500, ex.Message); // Retorna 500 em caso de erro interno
            }
        }

    }
}
