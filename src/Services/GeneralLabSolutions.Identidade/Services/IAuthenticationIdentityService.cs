using GeneralLabSolutions.Identidade.Data;
using GeneralLabSolutions.Identidade.Model;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace GeneralLabSolutions.Identidade.Services
{
    public interface IAuthenticationIdentityService 
    {

        /// <summary>
        /// Gera um token JWT para um usuário com base no e-mail.
        /// </summary>
        /// <param name="email">E-mail do usuário.</param>
        /// <returns>Um objeto contendo o token de resposta do usuário.</returns>
        Task<UsuarioRespostaLogin> GerarJwt(string email);

        /// <summary>
        /// Obtém um Refresh Token pelo seu identificador GUID.
        /// </summary>
        /// <param name="refreshToken">Identificador do Refresh Token.</param>
        /// <returns>Objeto RefreshToken se for válido, caso contrário, null.</returns>
         Task<RefreshToken?> ObterRefreshToken(Guid refreshToken);

        /// <summary>
        /// Obtém as claims associadas a um usuário.
        /// </summary>
        /// <param name="claims">Coleção de claims adicionais.</param>
        /// <param name="user">Usuário do Identity.</param>
        /// <returns>ClaimsIdentity com as claims do usuário.</returns>
         Task<ClaimsIdentity> ObterClaimsUsuario(ICollection<Claim> claims, ApplicationUser user);

        /// <summary>
        /// Codifica um token JWT com base nas claims fornecidas.
        /// </summary>
        /// <param name="identityClaims">ClaimsIdentity contendo as claims do token.</param>
        /// <returns>String com o token JWT codificado.</returns>
         string CodificarToken(ClaimsIdentity identityClaims);

        /// <summary>
        /// Gera a resposta do token para um usuário autenticado.
        /// </summary>
        /// <param name="encodedToken">Token codificado JWT.</param>
        /// <param name="user">Usuário do Identity.</param>
        /// <param name="claims">Claims associadas ao usuário.</param>
        /// <param name="refreshToken">Refresh Token gerado.</param>
        /// <returns>Objeto UsuarioRespostaLogin com o token e informações adicionais.</returns>
          UsuarioRespostaLogin ObterRespostaToken(string encodedToken, IdentityUser user, IEnumerable<Claim> claims, RefreshToken refreshToken);

        /// <summary>
        /// Gera um novo Refresh Token para um usuário com base no e-mail.
        /// </summary>
        /// <param name="email">E-mail do usuário.</param>
        /// <returns>Objeto RefreshToken gerado.</returns>
         Task<RefreshToken> GerarRefreshToken(string email);
    }

    
}
