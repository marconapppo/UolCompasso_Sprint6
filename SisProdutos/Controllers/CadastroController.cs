using AutoMapper;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SisProdutos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace SisProdutos
{
    [ApiController]
    [Route("api/[controller]")]
    public class CadastroController : ControllerBase
    {
        private CadastroService _cadastroService;
        private readonly HttpClient _httpClient;
        private IMapper _mapper;

        public CadastroController(CadastroService cadastroService, HttpClient httpClient, IMapper mapper)
        {
            _cadastroService = cadastroService;
            _httpClient = httpClient;
            _mapper = mapper;
        }

        [HttpPost()]
        public async Task<IActionResult> CadastraUsuarioAsync(CreateUsuarioDto createUsuarioDto)
        {
            //criando usuario
            Result resultado = _cadastroService.CadastraUsuario(createUsuarioDto);
            if (resultado.IsFailed) return BadRequest(resultado.Errors);
            ////pegando o usuario criado
            var usuarioIdentity = _cadastroService.RetornaUsuarioAsync(createUsuarioDto.Username);
            //pequena conversão para retornar ao usuario somente id e nome
            Usuario usuario = new Usuario();
            usuario.Id = usuarioIdentity.Id;
            usuario.Username = usuarioIdentity.UserName;
            return CreatedAtAction(nameof(GetUsuario), new { nomeUsuario = usuario.Username }, usuario);
 

            
            ////criando cliente
            //CreateClienteDto createClienteDto = _mapper.Map<CreateClienteDto>(createUsuarioClienteDto);
            //var stringContent = new StringContent(JsonConvert.SerializeObject(createUsuarioClienteDto), Encoding.UTF8, "application/json");
            ////mandando resquest para criar cliente em SisCliente
            //var responseString = await _httpClient.PostAsync("https://localhost:5001/api/Cliente/" + usuario.Id, stringContent);

            //if (responseString.IsSuccessStatusCode) { return Ok(); }
            //return NotFound();
        }

        [HttpGet("{nomeUsuario}")]
        public IActionResult GetUsuario(string nomeUsuario)
        {
            var usuarioIdentity = _cadastroService.RetornaUsuarioAsync(nomeUsuario);
            //pequena conversão para retornar ao usuario somente id e nome
            Usuario usuario = new Usuario();
            usuario.Id = usuarioIdentity.Id;
            usuario.Username = usuarioIdentity.UserName;
            return Ok(usuario);
        }
        
    }
}
