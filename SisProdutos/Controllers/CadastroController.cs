using FluentResults;
using Microsoft.AspNetCore.Mvc;
using SisProdutos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace SisProdutos
{
    [ApiController]
    [Route("[controller]")]
    public class CadastroController : ControllerBase
    {
        private CadastroService _cadastroService;

        public CadastroController(CadastroService cadastroService)
        {
            _cadastroService = cadastroService;
        }

        [HttpPost]
        public IActionResult CadastraUsuario(CreateUsuarioDto createDto)
        {
            Result resultado = _cadastroService.CadastraUsuario(createDto);
            Console.WriteLine(resultado.Errors);
            if (resultado.IsFailed) return BadRequest(resultado.Errors);
            return Ok();
        }
    }
}
