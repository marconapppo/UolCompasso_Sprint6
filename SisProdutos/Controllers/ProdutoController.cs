using AutoMapper;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using SisProdutos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace SisProdutos
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutoController : ControllerBase
    {
        private IMapper _mapper;
        private UserContext _context;
        private ProdutoControllerFunctions _ProdutoFunctions;
        private readonly HttpClient _httpClient;


        public ProdutoController(IMapper mapper, UserContext context, HttpClient httpClient)
        {
            _mapper = mapper;
            _context = context;
            _ProdutoFunctions = new ProdutoControllerFunctions();
            _httpClient = httpClient;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CadastraProdutoAsync(CreateProdutoDto createDto)
        {
            try
            {
                Produto produto = _mapper.Map<Produto>(createDto);
                _context.Produtos.Add(produto);
                _context.SaveChanges();

                //pegando o ultimo produto, ou seja, aquela acabado de inserir
                produto = _context.Produtos
                           .OrderByDescending(p => p.Id)
                           .FirstOrDefault();

                //inserindo na tabela associativa
                List<ProdutoCidade> produtoCidadeList = new List<ProdutoCidade>();

                //pegando Ids da lista de Ceps de cidade
                var cidadeIdList = await _ProdutoFunctions.GetListCidadeIdsAsync(createDto.CepOpcionais, _httpClient, _context);

                //cadastrando para o tamanho do vetor de cidade do produto
                foreach (var cidadeId in cidadeIdList)
                {
                    ProdutoCidade pc = new ProdutoCidade();
                    pc.ProdutoId = produto.Id;
                    pc.CidadeId = cidadeId;
                    produtoCidadeList.Add(pc);
                }
                _context.ProdutoCidades.AddRange(produtoCidadeList);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetProduto), new { id = produto.Id }, produto);
            }
            catch(Exception e)
            {
                return NotFound();
            }
        }


        [HttpGet]
        public IActionResult GetProduto()
        {
            List<ReadProdutoDto> produtoDto = new List<ReadProdutoDto>();

            foreach (var produto in _context.Produtos)
            {
                produtoDto.Add(_mapper.Map<ReadProdutoDto>(produto));
            }
            return Ok(produtoDto);
        }

        [HttpGet("{filtro}")]
        public IActionResult BuscaProduto([FromQuery] string Nome = null, string PalavraChave = null, string Descricao = null,
            string Categoria = null, bool? AscPreco = null)
        {
            List<Produto> produtoList = new List<Produto>();

            //para caso um deles for null
            produtoList = _ProdutoFunctions.AjustarProdutoList(produtoList, Nome, PalavraChave, Descricao, Categoria, AscPreco, _context);

            if (produtoList.Count == 0)
            {
                return NotFound();
            }
            return Ok(produtoList);
        }

        [HttpGet("compra/{idProduto}")]
        [Authorize]
        public async Task<IActionResult> GetFreteProdutoAsync(int idProduto)
        {
            ClienteProduto clienteProduto = new ClienteProduto();
            try
            {
                //pega o id da cidade principal do 
                var usuario = _ProdutoFunctions.GetUsuario(User.Identity.Name, _context);
                int idCliente = await _ProdutoFunctions.GetIdClienteAsync(usuario, _httpClient);
                //colocando valores no cliente produto
                clienteProduto.IdCliente = idCliente;
                clienteProduto.IdProduto = idProduto;
                int idCidadeCliente = await _ProdutoFunctions.GetIdPrincipalClienteAsync(idCliente, _httpClient);
                //pega o preço do produto
                float precoProduto = _ProdutoFunctions.GetPrecoProduto(idProduto, _context);
                //pega a cidade do cliente
                var cidadeCliente = await _ProdutoFunctions.GetCidadeClienteAsync(idCidadeCliente, _httpClient);
                //pega as cidades do produto
                var cidadesList = _ProdutoFunctions.PegaCidadesProduto(_context, idProduto);
                //compara para saber se haverá frete, ou seja, cidade de cliente é diferente de produto
                string resposta = _ProdutoFunctions.GetRespostaFrete(cidadesList, cidadeCliente.Nome, precoProduto);
                //caso tudo esta certo, cliente não tera erros
                clienteProduto.ErrosList = null;
                //registrando em auditoria
                var stringContent = new StringContent(JsonConvert.SerializeObject(clienteProduto), Encoding.UTF8, "application/json");
                await _httpClient.PostAsync("https://localhost:7001/api/Auditoria", stringContent);
                return Ok(resposta);
            }
            catch(Exception e)
            {
                List<string> erros = new List<string>();
                erros.Add(e.Message);
                clienteProduto.ErrosList = erros;
                //registrando em auditoria
                var stringContent = new StringContent(JsonConvert.SerializeObject(clienteProduto), Encoding.UTF8, "application/json");
                await _httpClient.PostAsync("https://localhost:7001/api/Auditoria", stringContent);

                return NotFound();
            }
        }


    }
}
