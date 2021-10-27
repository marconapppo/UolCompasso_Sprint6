using AutoMapper;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
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
        private ClienteControllerFunctions _clienteFunctions;


        public ClienteController(IMapper mapper, HttpClient httpClient, PaisContext context)
        {
            _mapper = mapper;
            _httpClient = httpClient;
            _context = context;
            _clienteFunctions = new ClienteControllerFunctions();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateClienteAsync([FromBody] CreateClienteDTO clienteDTO)
        {
            //validando cliente
            var validator = new CreateClienteValidator();
            ValidationResult results = validator.Validate(clienteDTO);
            IList<ValidationFailure> failures = results.Errors;
            if (!results.IsValid) { return BadRequest(failures); }
            //pegando a cidade da pessoa pelo CEP, utilizando o site abaixo
            clienteDTO.Cep = clienteDTO.Cep.Replace("-", "");
            var catalog = await _clienteFunctions.GetCepPeloSiteAsync(clienteDTO.Cep, clienteDTO.Logradouro, clienteDTO.Bairro, _httpClient);
            //cruzando os valores de cidade com os do banco
            Cidade cidade = _context.Cidades.FirstOrDefault(cidade => cidade.Nome == catalog.localidade);
            if (cidade != null)
            {
                //cadastrando cliente
                Cliente cliente = _mapper.Map<Cliente>(clienteDTO);
                //pegando usuario
                var responseString = await _httpClient.GetStringAsync("https://localhost:6002/api/Cadastro/" + User.Identity.Name);
                var usuario = JsonConvert.DeserializeObject<Usuario>(responseString);
                //conferindo caso exista um cliente ja cadastrado
                if (_context.Clientes.FirstOrDefault(cliente => cliente.Id == usuario.Id) != null) { return Conflict(); }
                //trocando id do cliente para o msm do usuario
                cliente.Id = usuario.Id;
                //salvando no banco o cliente
                _context.Clientes.Add(cliente);
                _context.SaveChanges();
                //inserindo na tabela associativa
                var clienteCidadeList = await _clienteFunctions.GetAssociativaCLienteCidadeAsync(cliente.Id,cidade.Id,clienteDTO.CepOpcionais, _context,_httpClient);
                _context.ClienteCidade.AddRange(clienteCidadeList);
                _context.SaveChanges();
                return CreatedAtAction(nameof(RecuperaClientePorId), new { id = cliente.Id }, cliente);
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
            var catalog = await _clienteFunctions.GetCepPeloSiteAsync(clienteDto.Cep, clienteDto.Logradouro, clienteDto.Bairro, _httpClient);
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
