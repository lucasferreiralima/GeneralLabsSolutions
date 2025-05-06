using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GeneralLabSolutions.WebApiCore.Identidade
{
    public class RequisitoClaimFilter : IAuthorizationFilter
    {
        private readonly Claim _claim;
        private readonly string? _role;

        public RequisitoClaimFilter(Claim claim, string? role)
        {
            _claim = claim;
            _role = role;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity!.IsAuthenticated)
            {
                context.Result = new StatusCodeResult(401);
                return;
            }

            // Verificar se o usuário é SuperAdmin (acesso irrestrito)
            if (CustomAuthorization.EhSuperAdmin(context.HttpContext))
            {
                return; // Permite o acesso
            }

            if ((!CustomAuthorization.ValidarClaimsUsuario(context.HttpContext, _claim.Type, _claim.Value)
                && !CustomAuthorization.PossuiRole(context.HttpContext, _role!)))
            {
                context.Result = new StatusCodeResult(403);
            }
        }
    }
}