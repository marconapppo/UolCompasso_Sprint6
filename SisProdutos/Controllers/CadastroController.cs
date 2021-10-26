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
        public async Task<IActionResult> CadastraUsuarioAsync(CreateUsuarioClienteDto createUsuarioClienteDto)
        {
            //criando usuario
            CreateUsuarioDto createUsuarioDto = _mapper.Map<CreateUsuarioDto>(createUsuarioClienteDto);
            Result resultado = _cadastroService.CadastraUsuario(createUsuarioDto);
            if (resultado.IsFailed) return BadRequest(resultado.Errors);
            else { return Ok(); }
            //pegando o usuario criado
            var usuario = _cadastroService.RetornaUsuarioAsync(createUsuarioDto.Username);
            //criando cliente
            CreateClienteDto createClienteDto = _mapper.Map<CreateClienteDto>(createUsuarioClienteDto);
            var stringContent = new StringContent(JsonConvert.SerializeObject(createUsuarioClienteDto), Encoding.UTF8, "application/json");
            //mandando resquest para criar cliente em SisCliente
            var responseString = await _httpClient.PostAsync("https://localhost:5001/api/Cliente/" + usuario.Id, stringContent);
            if (responseString.IsSuccessStatusCode) { return Ok(); }
            return NotFound();
        }


        
    }
}
