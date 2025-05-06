using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using GeneralLabSolutions.Domain.Entities;
using GeneralLabSolutions.InfraStructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VelzonModerna.ViewModels;

namespace VelzonModerna.Controllers
{
    public class AgendaEventosController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _imapper;
        private readonly ILogger<AgendaEventosController> _logger;
        //private readonly INotyfService _notifyService;

        public AgendaEventosController(AppDbContext context, IMapper imapper, ILogger<AgendaEventosController> logger)
        {
            _context = context;
            _imapper = imapper;
            _logger = logger;
            //_notifyService = notifyService;
        }

        public IActionResult Index(DateTime? eventDate)
        {
            ViewBag.EventDate = eventDate ?? DateTime.Now;
            //if (TempData ["SuccessMessage"] != null)
            //    _notifyService.Success("Evento salvo com sucesso!");

            return View();
        }

        public JsonResult GetEvents()
        {
            var agendaEventos = (from evento in _context.AgendaEventos
                                 join participante in _context.Participante
                                 on evento.ParticipanteId equals participante.Id
                                 select new
                                 {
                                     evento.Id,
                                     NomeParticipante = participante.Name,
                                     evento.Title,
                                     evento.Start,
                                     evento.End,
                                     evento.Description,
                                     evento.Color,
                                     evento.AllDay,
                                 }).ToArray();

            return Json(agendaEventos);
        }

        [HttpGet]
        public async Task<IActionResult> AgendaFilter()
        {
            var listAgendaEventos = await _context.Set<AgendaEventos>()
                .Include(e => e.Participante)
                .AsNoTracking().ToListAsync();

            return View(listAgendaEventos);
        }


        [HttpGet]
        public IActionResult GetEventForEdit(Guid id)
        {
            var evento = _context.AgendaEventos
                .Include(a => a.Participante)
                .FirstOrDefault(a => a.Id == id);

            if (evento == null)
                return NotFound();

            var viewModel = new CreateEditAgendaEventoViewModel
            {
                Id = evento.Id,
                ParticipanteId = evento.ParticipanteId,
                NomeParticipante = evento.Participante?.Name, // se desejar exibir
                AllDay = evento.AllDay,
                Color = evento.Color,
                Description = evento.Description,
                End = evento.End,
                Start = evento.Start,
                Title = evento.Title
            };

            PrepareDropdownsData(viewModel);
            return PartialView("PartialViews/_EventCalendarModal", viewModel);
        }


        [HttpPost]
        //[IgnoreAntiforgeryToken]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateEventDates(Guid id, DateTime start, DateTime? end)
        {
            try
            {
                var evento = _context.AgendaEventos.Find(id);
                if (evento == null)
                    return NotFound("Evento não encontrado.");

                // Lógica de validação no servidor:
                // se for passado, se for data antiga, etc., decida o que fazer

                // Atualiza as datas
                evento.Start = start;
                evento.End = end;

                _context.SaveChanges();
                return Ok(new { success = true });
            } catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar datas do evento.");
                return BadRequest(new { success = false, message = "Erro ao atualizar datas." });
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        //[IgnoreAntiforgeryToken]
        public IActionResult Save(CreateEditAgendaEventoViewModel model)
        {
            if (ModelState.IsValid)
            {
                AgendaEventos evento;
                // Verifica se é uma criação ou edição
                if (model.Id == Guid.Empty)
                {
                    evento = new AgendaEventos
                    {
                        ParticipanteId = model.ParticipanteId,
                        Title = model.Title,
                        Description = model.Description,
                        Start = model.Start,
                        End = model.End,
                        Color = model.Color,
                        AllDay = model.AllDay,
                    };

                    _context.AgendaEventos.Add(evento);
                } else
                {
                    evento = _context.AgendaEventos.Find(model.Id);
                    if (evento == null)
                    {
                        //_notifyService.Error("Evento não encontrado.");
                        return RedirectToAction(nameof(Index));
                    }

                    //_imapper.Map(model, evento); 

                    // Atualiza as propriedades do OBJETO encontrado
                    evento.ParticipanteId = model.ParticipanteId;
                    evento.Title = model.Title;
                    evento.Description = model.Description;
                    evento.Start = model.Start;
                    evento.End = model.End;
                    evento.Color = model.Color;
                    evento.AllDay = model.AllDay;

                }


                try
                {
                    _context.SaveChanges();
                    TempData ["SuccessMessage"] = "Evento salvo com sucesso!";
                    return RedirectToAction(nameof(Index));
                } catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao salvar evento");
                    //_notifyService.Error("Erro ao salvar evento");
                    return RedirectToAction(nameof(Index));
                }
            }

            // Em caso de falha, prepara novamente os dados para os dropdowns antes de reexibir a View
            PrepareDropdownsData(model);
            return View("CreateModal", model); // Ou a view que você usa para mostrar o form
        }



        [HttpGet]
        public IActionResult EventDetails(Guid id)
        {
            var evento = _context.AgendaEventos
                .Include(e => e.Participante)
                .FirstOrDefault(e => e.Id == id);

            if (evento == null)
            {
                return NotFound();
            }

            var viewModel = new CreateEditAgendaEventoViewModel
            {
                // Copie as propriedades necessárias de evento para viewModel
                Id = id,
                NomeParticipante = evento.Participante.Name,
                Title = evento.Title,
                Start = evento.Start,
                End = evento.End,
                Description = evento.Description

            };

            return PartialView("PartialViews/_EventDetailsModal", viewModel);
        }


        public IActionResult CreateModal()
        {
            var viewModel = new CreateEditAgendaEventoViewModel();
            // Preencha viewModel.Pacientes, viewModel.Medicos, etc., aqui
            PrepareDropdownsData(viewModel); // Reaproveitando o método sugerido anteriormente
            return PartialView("PartialViews/_EventCalendarModal", viewModel);
        }

        private void PrepareDropdownsData(CreateEditAgendaEventoViewModel model)
        {
            model.Participantes = _context.Participante.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name
            }).ToList();


        }

    }
}
