using System.Linq;
using Microsoft.AspNetCore.Http;

namespace GeneralLabSolutions.WebApiCore.Identidade
{
    public static class CustomAuthorization
    {
        public static bool EhSuperAdmin(HttpContext context)
        {
            return PossuiRole(context, "SuperAdmin");
        }

        public static bool ValidarClaimsUsuario(HttpContext context, string claimName, string claimValue)
        {
            return context.User.Identity!.IsAuthenticated &&
                   context.User.Claims.Any(c => c.Type == claimName && c.Value.Contains(claimValue));
        }

        public static bool PossuiRole(HttpContext context, string role)
        {
            return context.User.IsInRole(role);
        }
    }
}