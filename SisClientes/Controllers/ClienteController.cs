using AutoMapper;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
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
    public class ClienteController : Controller
    {
        private PaisContext _context;
        private IMapper _mapper;
        private readonly HttpClient _httpClient;

        public ClienteController(IMapper mapper, HttpClient httpClient, PaisContext context)
        {
            _mapper = mapper;
            _httpClient = httpClient;
            _context = context;
        }

        [HttpPost("{idCliente?}")]
        public async Task<IActionResult> CreateClienteAsync([FromBody] CreateClienteDTO clienteDTO, int? idCliente = null)
        {
            Console.WriteLine(idCliente);
            //validando cliente
            var validator = new CreateClienteValidator();
            ValidationResult results = validator.Validate(clienteDTO);
            IList<ValidationFailure> failures = results.Errors;
            if (!results.IsValid) { return BadRequest(failures); }
            //pegando a cidade da pessoa pelo CEP, utilizando o site abaixo
            clienteDTO.Cep = clienteDTO.Cep.Replace("-", "");
            var responseString = await _httpClient.GetStringAsync("https://viacep.com.br/ws/" + clienteDTO.Cep + "/json/");
            var catalog = JsonConvert.DeserializeObject<CatalogCep>(responseString);
            //recebendo valores de cidade e catalog, e caso logradouro ou bairro seja null, insere o que veio no corpo
            if (!string.IsNullOrEmpty(catalog.logradouro)) { clienteDTO.Logradouro = catalog.logradouro; }
            if (!string.IsNullOrEmpty(catalog.bairro)) { clienteDTO.Bairro = catalog.bairro; }
            //cruzando os valores de cidade com os do banco
            Cidade cidade = _context.Cidades.FirstOrDefault(cidade => cidade.Nome == catalog.localidade);
            if (cidade != null)
            {
                //cadastrando cliente
                Cliente cliente = _mapper.Map<Cliente>(clienteDTO);
                if(idCliente != null) { cliente.Id = int.Parse(idCliente.ToString()); }
                Console.WriteLine(cliente.Id);
                //o dto nao possui  cidadeId, então eu insero direto
                cliente.CidadeId = cidade.Id;
                //_context.Database.AutoTransactionsEnabled;
                _context.Clientes.Add(cliente);
                _context.SaveChanges();
                return RecuperaClientePorId(cliente.Id);
            }
            return NotFound();
        }


        [HttpGet]
        public IActionResult GetCliente()
        {
            List<ReadClienteDTO> clienteDto = new List<ReadClienteDTO>();
            foreach (var cliente in _context.Clientes)
            {
                clienteDto.Add(_mapper.Map<ReadClienteDTO>(cliente));
            }
            return Ok(clienteDto);
        }

        [HttpGet("{id}")]
        public IActionResult RecuperaClientePorId(int id)
        {
            Cliente cliente = _context.Clientes.FirstOrDefault(cliente => cliente.Id == id);
            if (cliente != null)
            {
                ReadClienteDTO ClienteDto = _mapper.Map<ReadClienteDTO>(cliente);

                return Ok(ClienteDto);
            }
            return NotFound();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizaClienteAsync(int id, [FromBody] UpdateClienteDTO clienteDto)
        {
            //validando cliente
            var validator = new UpdateClienteValidator();
            ValidationResult results = validator.Validate(clienteDto);
            IList<ValidationFailure> failures = results.Errors;
            if (!results.IsValid) { return BadRequest(failures); }
            //pegando a cidade da pessoa pelo CEP, utilizando o site abaixo
            clienteDto.Cep = clienteDto.Cep.Replace("-", "");
            var responseString = await _httpClient.GetStringAsync("https://viacep.com.br/ws/" + clienteDto.Cep + "/json/");
            var catalog = JsonConvert.DeserializeObject<CatalogCep>(responseString);
            //recebendo valores de cidade e catalog, e caso logradouro ou bairro seja null, insere o que veio no corpo
            if (!string.IsNullOrEmpty(catalog.logradouro)) { clienteDto.Logradouro = catalog.logradouro; }
            if (!string.IsNullOrEmpty(catalog.bairro)) { clienteDto.Bairro = catalog.bairro; }
            //autalizando cliente
            Cliente cliente = _context.Clientes.FirstOrDefault(cliente => cliente.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }
            //cruzando os valores de cidade com os do banco
            Cidade cidade = _context.Cidades.FirstOrDefault(cidade => cidade.Nome == catalog.localidade);
            if (cidade != null)
            {
                //o dto nao possui  cidadeId, então eu insero direto
                cliente.CidadeId = cidade.Id;
                //alterando valores de cliente
                _mapper.Map(clienteDto, cliente);
                _context.SaveChanges();
                return RecuperaClientePorId(cliente.Id);
            }
            return NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletaCliente(int id)
        {
            Cliente cliente = _context.Clientes.FirstOrDefault(cliente => cliente.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }
            _context.Remove(cliente);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
