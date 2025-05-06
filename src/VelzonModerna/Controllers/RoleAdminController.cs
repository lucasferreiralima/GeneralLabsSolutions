using GeneralLabSolutions.Identidade.Dtos;
using Microsoft.AspNetCore.Mvc;
using VelzonModerna.Services;

namespace VelzonModerna.Controllers
{
    
        [Route("RoleAdmin")]
        public class RoleAdminController : Controller
        {
            private readonly RoleAdminService _roleAdminService;

            public RoleAdminController(RoleAdminService roleAdminService)
            {
                _roleAdminService = roleAdminService;
            }

            // Listar todas as roles
            [HttpGet("Index")]
            public async Task<IActionResult> Index()
            {
                var roles = await _roleAdminService.GetRolesAsync();
                return View(roles);
            }

            // Formulário de criação de role
            [HttpGet("Create")]
            public IActionResult Create()
            {
                return View();
            }

            // Criar role
            [HttpPost("Create")]
            public async Task<IActionResult> Create(CriarRoleDto roleDto)
            {
                if (!ModelState.IsValid) return View(roleDto);

                var result = await _roleAdminService.CreateRoleAsync(roleDto);
                if (!result) ModelState.AddModelError(string.Empty, "Erro ao criar role.");

                return RedirectToAction("Index");
            }

            // Excluir role
            [HttpPost("Delete/{roleName}")]
            public async Task<IActionResult> Delete([FromRoute] string roleName)
            {
                var result = await _roleAdminService.DeleteRoleAsync(roleName);
                if (!result) ModelState.AddModelError(string.Empty, "Erro ao excluir role.");

                return RedirectToAction("Index");
            }

            // Gerenciar usuários em uma role
            [HttpGet("ManageUsers/{roleName}")]
            public async Task<IActionResult> ManageUsers([FromRoute] string roleName)
            {
                var users = await _roleAdminService.GetUsersByRoleAsync(roleName);
                return View(users);
            }

            // Adicionar usuário a uma role
            [HttpPost("AddUserToRole")]
            public async Task<IActionResult> AddUserToRole([FromForm] AdicionarUsuarioRoleDto dto)
            {
                var result = await _roleAdminService.AddUserToRoleAsync(dto);
                if (!result) ModelState.AddModelError(string.Empty, "Erro ao adicionar usuário à role.");

                return RedirectToAction("Index");
            }
        }
    }

