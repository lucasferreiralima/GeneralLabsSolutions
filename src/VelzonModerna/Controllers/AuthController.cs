using GeneralLabSolutions.Identidade.Model;
using Microsoft.AspNetCore.Mvc;
using VelzonModerna.Services;
using VelzonModerna.ViewModels;

namespace VelzonModerna.Controllers
{
    [Route("Auth")]
    public class AuthController : Controller
    {
        private readonly IAutenticacaoService _autenticacaoService;

        public AuthController(IAutenticacaoService autenticacaoService)
        {
            _autenticacaoService = autenticacaoService;
        }

        // Exibir tela de login
        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View();
        }

        // Realizar login
        [HttpPost("Login")]
        public async Task<IActionResult> Login(GeneralLabSolutions.Identidade.Model.UsuarioLogin usuarioLogin)
        {
            if (!ModelState.IsValid) return View(usuarioLogin);

            // Converter manualmente para o tipo esperado
            var usuarioLoginViewModel = new VelzonModerna.ViewModels.UsuarioLogin
            {
                Email = usuarioLogin.Email,
                Senha = usuarioLogin.Senha
            };

            var response = await _autenticacaoService.Login(usuarioLoginViewModel);


            if (response == null)//|| response.ResponseResult?.Errors?.Count > 0)
            {
                ModelState.AddModelError(string.Empty, "Usuário ou senha inválidos.");
                return View(usuarioLogin);
            }

            // Salvar token na sessão
            HttpContext.Session.SetString("AuthToken", response.AccessToken);

            return RedirectToAction("Index", "Home");
        }

        // Exibir tela de registro
        [HttpGet("Register")]
        public IActionResult Register()
        {
            return View();
        }

        //// Registrar usuário
        //[HttpPost("Register")]
        //public async Task<IActionResult> Register(UsuarioRegistro usuarioRegistro)
        //{
        //    if (!ModelState.IsValid) return View(usuarioRegistro);

        //    var response = await _autenticacaoService.Registro(usuarioRegistro);


        //    if (response == null || response.ResponseResult?.Errors?.Count > 0)
        //    {
        //        ModelState.AddModelError(string.Empty, "Erro ao registrar usuário.");
        //        return View(usuarioRegistro);
        //    }

        //    return RedirectToAction("Login");
        //}

//        // Registrar usuário
//[HttpPost("Register")]
//public async Task<IActionResult> Register(GeneralLabSolutions.Identidade.Model.UsuarioRegistro usuarioRegistro)
//{
//    if (!ModelState.IsValid) return View(usuarioRegistro);

//    // 🔹 Converter manualmente para o tipo esperado pelo serviço de autenticação
//    var UsuarioRegistroViewModel = new GeneralLabSolutions.Identidade.Model.UsuarioRegistro
//    {
//        Apelido = usuarioRegistro.Apelido,
//        NomeCompleto = usuarioRegistro.NomeCompleto,
//        DataNascimento = usuarioRegistro.DataNascimento,
//        ImgProfilePath = usuarioRegistro.ImgProfilePath,
//        Email = usuarioRegistro.Email,
//        Senha = usuarioRegistro.Senha,
//        SenhaConfirmacao = usuarioRegistro.SenhaConfirmacao
//    };

//    // 🔹 Chamar o serviço com o DTO convertido
//    var response = await _autenticacaoService.Registro(UsuarioRegistroViewModel);

//    if (response == null || response.ResponseResult?.Errors?.Count > 0)
//    {
//        ModelState.AddModelError(string.Empty, "Erro ao registrar usuário.");
//        return View(UsuarioRegistroViewModel);
//    }

//    return RedirectToAction("Login");
//}
//        // Logout
//        [HttpGet("Logout")]
//        public IActionResult Logout()
//        {
//            HttpContext.Session.Remove("AuthToken");
//            return RedirectToAction("Login");
//        }




        // Registrar usuário
[HttpPost("Register")]
public async Task<IActionResult> Register(UsuarioRegistroViewModel usuarioRegistroViewModel)
{
    if (!ModelState.IsValid) return View(usuarioRegistroViewModel);



    // 🔹 Converter manualmente para o tipo esperado pelo serviço de autenticação
    var usuarioRegistro = new ViewModels.UsuarioRegistro
    {
        Apelido = usuarioRegistroViewModel.Apelido,
        NomeCompleto = usuarioRegistroViewModel.NomeCompleto,
        DataNascimento = usuarioRegistroViewModel.DataNascimento,
        ImgProfilePath = usuarioRegistroViewModel.ImgProfilePath,
        Email = usuarioRegistroViewModel.Email,
        Senha = usuarioRegistroViewModel.Senha,
        SenhaConfirmacao = usuarioRegistroViewModel.SenhaConfirmacao
    };

    // 🔹 Chamar o serviço com o DTO convertido
    var response = await _autenticacaoService.Registro(usuarioRegistro);

    if (response == null )//|| response.ResponseResult?.Errors?.Count > 0)
    {
        ModelState.AddModelError(string.Empty, "Erro ao registrar usuário.");
        return View(usuarioRegistroViewModel);
    }

    return RedirectToAction("Login");
}
    }
}
