using GeneralLabSolutions.InfraStructure.Data;
using GeneralLabSolutions.WebAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GeneralLabSolutions.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendedorController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VendedorController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("lista-vendedores")]
        [ProducesResponseType(typeof(IEnumerable<VendedorGridDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ListaDeVendedores()
        {
            try
            {
                var listaVendedor = await _context.Vendedor
                    .AsSplitQuery()
                    .AsNoTracking()
                    .Select(item => new VendedorGridDto
                    {
                        Id = item.Id,
                        Nome = item.Nome,
                        Documento = item.Documento,
                        Email = item.Email,
                        TipoDePessoa = item.TipoDePessoa
                    }).ToListAsync();

                if (!listaVendedor.Any())
                {
                    // Retorna 404 se nenhum vendedor for encontrado
                    return NotFound("Nenhum vendedor encontrado.");
                }

                // Retorna 200 com a lista de vendedores
                return Ok(listaVendedor);
            } catch (Exception ex)
            {
                // Log do erro aqui
                return StatusCode(500, ex.Message); // Retorna 500 em caso de erro interno
            }
        }


    }

}
