using GeneralLabSolutions.Domain.Extensions.Helpers.Generics;
using GeneralLabSolutions.Domain.Interfaces;
using GeneralLabSolutions.InfraStructure.Data;
using GeneralLabSolutions.WebAPI.DTOs;
using GeneralLabSolutions.WebAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GeneralLabSolutions.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {

        private readonly IProdutoRepository _produtoRepository;
        private readonly IPaginationService _paginationService;

        public ProdutoController(IProdutoRepository produtoRepository, 
                                 IPaginationService paginationService)
        {
            _produtoRepository = produtoRepository;
            _paginationService = paginationService;
        }

        [HttpGet("produtos-paginados")]
        [ProducesResponseType(typeof(PagedResult<ProdutoGridDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagedResult<ProdutoGridDto>>> ListaProdutosPaginados(
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 8,
            [FromQuery] string? query = null)
        {
            try
            {
                var pagedResult = await _produtoRepository.ObterTodosPaginado(pageIndex, pageSize, query);

                if (pagedResult.List == null || !pagedResult.List.Any())
                {
                    return NotFound("Nenhum produto encontrado.");
                }

                // Mapear os produtos para ProdutoGridDto
                var dtoList = pagedResult.List.Select(produto => new ProdutoGridDto
                {
                    Id = produto.Id,
                    Codigo = produto.Codigo,
                    Descricao = produto.Descricao,
                    Ncm = produto.Ncm,
                    ValorUnitario = produto.ValorUnitario,
                    StatusDoProduto = produto.StatusDoProduto,
                    DataDeValidade = produto.DataDeValidade
                }).ToList();

                // Retornar o PagedResult com DTOs
                var dtoPagedResult = new PagedResult<ProdutoGridDto>
                {
                    List = dtoList,
                    TotalResults = pagedResult.TotalResults,
                    PageIndex = pagedResult.PageIndex,
                    PageSize = pagedResult.PageSize,
                    TotalPages = pagedResult.TotalPages,
                    HasPrevious = pagedResult.HasPrevious,
                    HasNext = pagedResult.HasNext,
                    Query = pagedResult.Query
                };

                return Ok(_paginationService.AddPaginationMetadata(dtoPagedResult));

            } catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


    }
}
