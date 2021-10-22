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

        [HttpPost("{idCliente}")]
        public IActionResult CadastraUsuario(CreateUsuarioDto createDto,int idCliente)
        {
            Result resultado = _cadastroService.CadastraUsuario(createDto, idCliente);
            if (resultado.IsFailed) return BadRequest(resultado.Errors);
            return Ok();
        }
    }
}
