using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;



namespace SisProdutos
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private LoginService _loginService;

        public LoginController(LoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost]
        public IActionResult LogaUsuario(LoginRequest request)
        {

            var token = _loginService.GenerateToken(request);
            Console.WriteLine(User.Identity.Name);
            return Ok(token);

            //Result resultado = _loginService.LogaUsuario(request);
            //if (resultado.IsFailed) return Unauthorized(resultado.Errors);
            //return Ok(resultado.Successes);
        }

        [HttpGet]
        [Authorize]
        public void teste()
        {
            //var user = UserRepository.Get("batman", "robin");
            //string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Console.WriteLine(User.Identity.Name);
            //Console.WriteLine(userId);
        }

        [HttpGet]
        [Route("authenticated")]
        [Authorize]
        public string Authenticated() => String.Format("Autenticado - {0}", User.Identity.Name);

    }
}
