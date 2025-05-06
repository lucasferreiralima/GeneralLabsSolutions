using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace GeneralLabSolutions.WebApiCore.Identidade
{
    public class ClaimsAuthorizeAttribute : TypeFilterAttribute
    {
        public ClaimsAuthorizeAttribute(string claimName, string claimValue, string? role = null)
            : base(typeof(RequisitoClaimFilter))
        {
            Arguments = new object [] { new Claim(claimName, claimValue), role };
        }
    }
}