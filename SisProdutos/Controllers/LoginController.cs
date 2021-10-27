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




    }
}
