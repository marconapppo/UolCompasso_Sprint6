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
        public IActionResult CadastraProduto(CreateProdutoDto createDto)
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

            //cadastrando para o tamanho do vetor de cidade do produto
            for (int i=0; i< createDto.CidadeId.Length; i++)
            {
                ProdutoCidade pc = new ProdutoCidade();
                pc.ProdutoId = produto.Id;
                pc.CidadeId = createDto.CidadeId[i];
                produtoCidadeList.Add(pc);
            }
            _context.ProdutoCidades.AddRange(produtoCidadeList);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetProduto), new { id = produto.Id }, produto);
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

        [HttpGet("{idCidadeCliente}/{idProduto}")]
        public async Task<IActionResult> GetFreteProdutoAsync(int idCidadeCliente, int idProduto)
        {
            try
            {
                //pega o preço do produto
                var precoProduto = _context.Produtos.Where(p => p.Id == idProduto).Select(produto => produto.Preco).FirstOrDefault();
                precoProduto = (float)Math.Round(precoProduto);
                string resposta = "Frete de R$ 29,90, Valor:" + (precoProduto + 29.90).ToString("n2");
                //pega a cidade do cliente
                var responseString = await _httpClient.GetStringAsync("https://localhost:5001/api/Cidade/" + idCidadeCliente);
                var cidadeCliente = JsonConvert.DeserializeObject<Cidade>(responseString);
                //pega as cidades do produto
                var cidadesList = _ProdutoFunctions.PegaCidadesProduto(_context, idProduto);
                //compara para saber se haverá frete, ou seja, cidade de cliente é diferente de produto
                foreach (var cidadeProduto in cidadesList)
                {
                    if (cidadeProduto.Nome == cidadeCliente.Nome) { resposta = "Sem frete, Valor:" + precoProduto.ToString("n2"); }
                }
                //criando tabela Associativa ClienteProduto
                ClienteProduto clienteProduto = new ClienteProduto(1,idProduto,null);
                //registrando em auditoria
                var stringContent = new StringContent(JsonConvert.SerializeObject(clienteProduto), Encoding.UTF8, "application/json");
                await _httpClient.PostAsync("https://localhost:7001/api/Auditoria", stringContent);
                return Ok(resposta);
            }
            catch(Exception e)
            {
                return NotFound();
            }
        }


    }
}
