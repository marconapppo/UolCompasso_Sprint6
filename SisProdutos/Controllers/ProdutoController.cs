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


namespace SisProdutos
{
    [ApiController]
    [Route("[controller]")]
    public class ProdutoController : ControllerBase
    {
        private IMapper _mapper;
        private UserContext _context;
        private ProdutoControllerFunctions ProdutoFunctions;
        private readonly HttpClient _httpClient;

        public ProdutoController(IMapper mapper, UserContext context, HttpClient httpClient)
        {
            _mapper = mapper;
            _context = context;
            ProdutoFunctions = new ProdutoControllerFunctions();
            _httpClient = httpClient;
        }

        [HttpPost]
        public IActionResult CadastraProduto(CreateProdutoDto createDto)
        {

            Produto produto = _mapper.Map<Produto>(createDto);
            _context.Produtos.Add(produto);
            //_context.SaveChanges();

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
            foreach (var a in produtoCidadeList)
            {
                Console.WriteLine(a.ProdutoId + " " + a.CidadeId);
            }
            _context.ProdutoCidades.AddRange(produtoCidadeList);
            _context.SaveChanges();

            return Ok();
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

        [HttpGet("{Nome?}/{PalavraChave?}/{Descricao?}/{Categoria?}/{AscPreco?}")]
        public IActionResult BuscaProduto(string Nome = null, string PalavraChave = null, string Descricao = null,
            string Categoria = null, bool? AscPreco = null)
        {
            List<Produto> produtoList = new List<Produto>();

            //para caso um deles for null
            produtoList = ProdutoFunctions.AjustarProdutoList(produtoList, Nome, PalavraChave, Descricao, Categoria, AscPreco, _context);

            if (produtoList.Count == 0)
            {
                return NotFound();
            }
            return Ok(produtoList);
        }

        [HttpGet("{idCidadeCliente}/{idProduto}")]
        public async Task<IActionResult> GetFreteProdutoAsync(int idCidadeCliente, int idProduto)
        {
            //pega a cidade do cliente
            var responseString = await _httpClient.GetStringAsync("https://localhost:5001/api/Cidade/" + idCidadeCliente);
            var cidadeCliente = JsonConvert.DeserializeObject<Cidade>(responseString);
            //pega produto

            //retorna cidade de produto

            //retorna cidade de cliente

            //compara para saber se haverá frete, ou seja, cidade de cliente é diferente de produto
            Console.WriteLine(responseString);
            return Ok();
        }


    }
}
