using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SisClientes.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CidadeController : Controller
    {
        private PaisContext _context;
        private IMapper _mapper;
        private readonly HttpClient _httpClient;

        public CidadeController(IMapper mapper, PaisContext context, HttpClient httpClient)
        {
           _mapper = mapper;
            _context = context;
            _httpClient = httpClient;
        }

        [HttpPost]
        public IActionResult CreateCidade([FromBody] CreateCidadeDTO cidadeDTO)
        {
            //validando cidade
            var validator = new CreateCidadeValidator();
            ValidationResult results = validator.Validate(cidadeDTO);
            IList<ValidationFailure> failures = results.Errors;
            if (!results.IsValid) { return BadRequest(failures); }
            //inserindo cidade
            Cidade cidade = _mapper.Map<Cidade>(cidadeDTO);
            _context.Cidades.Add(cidade);
            _context.SaveChanges();
            return CreatedAtAction(nameof(RecuperaCidadePorId), new { id = cidade.Id }, cidade);
        }

        [HttpGet]
        public IActionResult GetCidade()
        {
            List<ReadCidadeDTO> cidadeDto = new List<ReadCidadeDTO>();
            
            foreach (var cidade in _context.Cidades)
            {
                cidadeDto.Add(_mapper.Map<ReadCidadeDTO>(cidade));
            }
            return Ok(cidadeDto);
        }

        [HttpGet("{id}")]
        public IActionResult RecuperaCidadePorId(int id)
        {
            Cidade cidade = _context.Cidades.FirstOrDefault(cidade => cidade.Id == id);
            if (cidade != null)
            {
                ReadCidadeDTO cidadeDto = _mapper.Map<ReadCidadeDTO>(cidade);
                return Ok(cidadeDto);
            }
            return NotFound();
        }

        [HttpGet("cep/{cep}")]
        public async Task<IActionResult> RecuperaCidadePorCepAsync(string cep)
        {
            try
            {
                //pega o nome da cidade
                var responseString = await _httpClient.GetStringAsync("https://viacep.com.br/ws/" + cep + "/json/");
                var catalog = JsonConvert.DeserializeObject<CatalogCep>(responseString);
                //cadastra cidade
                Cidade cidade = _context.Cidades.FirstOrDefault(cidade => cidade.Nome == catalog.localidade);
                if (cidade != null)
                {
                    ReadCidadeDTO cidadeDto = _mapper.Map<ReadCidadeDTO>(cidade);
                    return Ok(cidadeDto);
                }
                return NotFound();
            }
            catch
            {
                return NotFound();
            }
            
        }

        [HttpPut("{id}")]
        public IActionResult AtualizaCidade(int id, [FromBody] UpdateCidadeDTO cidadeDto)
        {
            //validando cidade
            var validator = new UpdateCidadeValidator();
            ValidationResult results = validator.Validate(cidadeDto);
            IList<ValidationFailure> failures = results.Errors;
            if (!results.IsValid) { return BadRequest(failures); }
            //atualizando cidade
            Cidade cidade = _context.Cidades.FirstOrDefault(cidade => cidade.Id == id);
            if (cidade == null)
            {
                return NotFound();
            }
            _mapper.Map(cidadeDto, cidade);
            _context.SaveChanges();
            return RecuperaCidadePorId(cidade.Id);
        }

        [HttpDelete("{id}")]
        public IActionResult DeletaCidade(int id)
        {
            Cidade cidade = _context.Cidades.FirstOrDefault(cidade => cidade.Id == id);
            if (cidade == null)
            {
                return NotFound();
            }
            _context.Remove(cidade);
            _context.SaveChanges();
            return NoContent();
        }

    }
}
