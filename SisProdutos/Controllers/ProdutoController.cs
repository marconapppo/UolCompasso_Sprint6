using AutoMapper;
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
    public class ProdutoController : ControllerBase
    {
        private IMapper _mapper;
        private UserContext _context;
        private ProdutoControllerFunctions ProdutoFunctions;

        public ProdutoController(IMapper mapper, UserContext context)
        {
            _mapper = mapper;
            _context = context;
            ProdutoFunctions = new ProdutoControllerFunctions();
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
            ProdutoCidade pc = new ProdutoCidade();
            pc.CidadeId = createDto.CidadeId;
            pc.ProdutoId = produto.Id;

            _context.ProdutoCidades.Add(pc);
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


    }
}
