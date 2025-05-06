using GeneralLabSolutions.Identidade.Dtos;
using Microsoft.AspNetCore.Mvc;
using VelzonModerna.Services;

namespace VelzonModerna.Controllers
{
    [Route("UserAdmin")]
    public class UserAdminController : Controller
    {
        private readonly UserAdminService _userAdminService;

        public UserAdminController(UserAdminService userAdminService)
        {
            _userAdminService = userAdminService;
        }

        // Listar usuários
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var users = await _userAdminService.GetUsersAsync();
            return View(users);
        }

        // Formulário de criação de usuário
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        // Criar usuário
        [HttpPost("Create")]
        public async Task<IActionResult> Create(CriarUsuarioDto usuario)
        {
            if (!ModelState.IsValid) return View(usuario);

            var result = await _userAdminService.CriarUsuarioAsync(usuario);
            if (!result) ModelState.AddModelError(string.Empty, "Erro ao criar usuário.");

            return RedirectToAction("Index");
        }

        // Exibir detalhes do usuário
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details([FromRoute] string id)
        {
            var user = await _userAdminService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }

        // Formulário de edição
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit([FromRoute] string id)
        {
            //var user = await _userAdminService.GetUserByIdAsync(id);
            //if (user == null) return NotFound();
            //return View(user);

            var user = await _userAdminService.GetUserByIdAsync(id);
            if (user == null) return NotFound();

            // ✅ Converter UserDto para AtualizarUsuarioDto
            var atualizarUsuarioDto = new AtualizarUsuarioDto
            {
                UserId = user.UserId,
                NomeCompleto = user.NomeCompleto,
                Email = user.Email,
                IsAtivo = user.EmailConfirmado == "Sim" // Convertendo "Sim" para bool
            };

            return View(atualizarUsuarioDto);

        }

        // Atualizar usuário
        //[HttpPost("Edit/{id}")]
        //public async Task<IActionResult> Edit(AtualizarUsuarioDto usuario)
        //{
        //    if (!ModelState.IsValid) return View(usuario);

        //    var result = await _userAdminService.AtualizarUsuarioAsync(usuario);
        //    if (!result) ModelState.AddModelError(string.Empty, "Erro ao atualizar usuário.");

        //    return RedirectToAction("Index");
        //}

        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> Edit([FromRoute] string id, [FromForm] AtualizarUsuarioDto usuario)
        {
            if (string.IsNullOrEmpty(id) || usuario == null || id != usuario.UserId)
                return BadRequest("ID inválido ou dados inconsistentes.");

            if (!ModelState.IsValid) return View(usuario);

            var result = await _userAdminService.AtualizarUsuarioAsync(usuario);
            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Erro ao atualizar usuário.");
                return View(usuario);
            }

            return RedirectToAction("Index");
        }

        // Confirmar exclusão
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userAdminService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }

        // Excluir usuário
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(string id)
        {
            var result = await _userAdminService.ExcluirUsuarioAsync(id);
            if (!result) ModelState.AddModelError(string.Empty, "Erro ao excluir usuário.");

            return RedirectToAction("Index");
        }

        [HttpGet("ManageRoles/{id}")]
        public async Task<IActionResult> ManageRoles([FromRoute] string id)
        {
            var roles = await _userAdminService.GetUserRolesAsync(id);
            var user = await _userAdminService.GetUserByIdAsync(id);

            if (user == null) return NotFound();

            var model = new ManageRolesDto
            {
                UserId = user.UserId,
                NomeCompleto = user.NomeCompleto,
                Roles = roles
            };

            return View(model);
        }

        [HttpPost("AddUserRole")]
        public async Task<IActionResult> AddUserRole([FromForm] AdicionarUsuarioRoleDto dto)
        {
            if (string.IsNullOrEmpty(dto.UserId) || string.IsNullOrEmpty(dto.RoleName))
                return BadRequest("ID do usuário e nome da role são obrigatórios.");

            var result = await _userAdminService.AdicionarUsuarioRoleAsync(dto);

            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Erro ao adicionar a role.");
                return RedirectToAction("ManageRoles", new { id = dto.UserId });
            }

            return RedirectToAction("Index");
        }






        //[HttpGet("GetClaims")]
        //public async Task<IActionResult> GetClaims()
        //{
        //    var claims = await _userAdminService.GetAllClaimsAsync();
        //    return View(claims);
        //}

        //[HttpPost("RemoveUserClaim")]
        //public async Task<IActionResult> RemoveUserClaim(string userId, string claimType, string claimValue)
        //{
        //    var result = await _userAdminService.RemoveUserClaimAsync(userId, claimType, claimValue);
        //    return result ? RedirectToAction("Index") : BadRequest("Erro ao remover claim.");
        //}

        [HttpPost("ToggleActivation")]
        public async Task<IActionResult> ToggleActivation(string userId, bool ativar)
        {
            var result = await _userAdminService.ToggleUserActivationAsync(userId, ativar);
            return result ? RedirectToAction("Index") : BadRequest("Erro ao ativar/inativar usuário.");
        }
    }
}
