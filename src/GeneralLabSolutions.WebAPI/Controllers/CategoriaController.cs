using GeneralLabSolutions.Domain.Entities;
using GeneralLabSolutions.InfraStructure.Data;
using GeneralLabSolutions.WebAPI.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GeneralLabSolutions.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriaController(AppDbContext context)
        {
            _context = context;
        }

        // CRUD - Create (Post), Read (Get), Update, Delete

        [HttpGet]
        [Route("lista-categorias")]
        [ProducesResponseType(typeof(IEnumerable<CategoriaProdutoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ListaCategoriaProduto()
        {

            try
            {
                var listacategoria = await _context.CategoriaProduto
                    .AsSplitQuery()
                    .AsNoTracking()
                    .Select(c => new CategoriaProdutoDto
                    {
                        Id = c.Id,
                        Descricao = c.Descricao!
                    }).ToListAsync();

                if (!listacategoria.Any())
                {
                    return NotFound("Nenhuma categoria encontrada!");
                }


                return Ok(listacategoria);

            } catch (Exception ex)
            {
                // Log do erro aqui
                return StatusCode(500, ex.Message);
            }

        }


    }
}
