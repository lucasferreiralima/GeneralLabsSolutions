using System.Security.Claims;
using GeneralLabSolutions.Identidade.Data;
using GeneralLabSolutions.Identidade.Dtos;
using GeneralLabSolutions.Identidade.Model;
using Microsoft.AspNetCore.Identity;

namespace GeneralLabSolutions.Identidade.Services
{
    public interface IRoleService
    {

        Task<ApplicationUser?> GetUserById(string userId);
        Task AdicionarClaim(ApplicationUser user, string tipo, string valor);
        Task<IdentityResult> CriarRoleAsync(string nomeRole);
        Task<IEnumerable<IdentityRole>> ObterTodasRolesAsync();
        Task<IList<string>> ObterRolesDoUsuarioAsync(string userId);
        Task<IEnumerable<ApplicationUser>> ObterUsuariosPorRoleAsync(string roleName);
        Task<IdentityResult> ExcluirRoleAsync(string roleName);
        UserDto MapearParaUserDto(ApplicationUser user);

    }
}
