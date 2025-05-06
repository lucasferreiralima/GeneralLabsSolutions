using System.Diagnostics;
using GeneralLabSolutions.Domain.Entities;
using GeneralLabSolutions.Domain.Extensions.Helpers.Generics;
using GeneralLabSolutions.Domain.Interfaces;
using GeneralLabSolutions.Domain.Mensageria;
using GeneralLabSolutions.InfraStructure.Data;
using GeneralLabSolutions.WebAPI.Application.Commands;
using GeneralLabSolutions.WebAPI.DTOs;
using GeneralLabSolutions.WebAPI.Services;
using GeneralLabSolutions.WebApiCore.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GeneralLabSolutions.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class ClienteController : MainController
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IPaginationService _paginationService;
        private readonly IMediatorHandler _mediatorHandler;

        public ClienteController(IClienteRepository clienteRepository,
                                 IPaginationService paginationService,
                                 IMediatorHandler mediatorHandler)
        {
            _clienteRepository = clienteRepository;
            _paginationService = paginationService;
            _mediatorHandler = mediatorHandler;
        }


        [HttpGet("lista-clientes-paginados")]
        [ProducesResponseType(typeof(PagedResult<ClienteGridDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagedResult<ClienteGridDto>>> ListaClientesPaginados(
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 8,
            [FromQuery] string? query = null)
        {
            try
            {
                // Obtem o resultado paginado de Clientes
                var models = await _clienteRepository.ObterTodosPaginado(pageIndex, pageSize, query);

                if (models.List == null || !models.List.Any())
                {
                    return NotFound("Nenhum cliente encontrado.");
                }

                // Faz a projeção dos Clientes para ClienteGridDto
                var dtoList = models.List.Select(item => new ClienteGridDto
                {
                    Id = item.Id,
                    Nome = item.Nome,
                    Documento = item.Documento,
                    Email = item.Email,
                    TipoDePessoa = item.TipoDePessoa,
                    StatusDoCliente = item.StatusDoCliente,
                    TipoDeCliente = item.TipoDeCliente
                }).ToList();

                // Cria um novo PagedResult<ClienteGridDto> mantendo a paginação original
                var dtoPagedResult = new PagedResult<ClienteGridDto>
                {
                    List = dtoList,
                    TotalResults = models.TotalResults,
                    PageIndex = models.PageIndex,
                    PageSize = models.PageSize,
                    TotalPages = models.TotalPages,
                    HasPrevious = models.HasPrevious,
                    HasNext = models.HasNext,
                    Query = models.Query
                };

                // Adiciona os metadados de paginação no header da resposta
                return Ok(_paginationService.AddPaginationMetadata(dtoPagedResult));

            } catch (Exception ex)
            {
                // Log do erro aqui
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("adicionar")]
        public async Task<IActionResult> AdicionarCliente(ClienteCommandDto dto)
        {
            var resultado = await _mediatorHandler.EnviarComando(new RegistrarClienteCommand
                (
                    dto.Id, dto.Nome!, dto.Documento!,
                    dto.TipoDePessoa, dto.TipoDeCliente, dto.StatusDoCliente,
                    dto.Email!
                ));

            return CustomResponse(resultado);
        }

    }
}