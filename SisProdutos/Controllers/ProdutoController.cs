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

        public ProdutoController(IMapper mapper, UserContext context)
        {
            _mapper = mapper;
            _context = context;
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

            Console.WriteLine(createDto.CidadeId + " " + produto.Id);
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

        [HttpGet("{id}")]
        public IActionResult RecuperaProdutoPorId(int id)
        {
            Produto produto = _context.Produtos.FirstOrDefault(produto => produto.Id == id);
            if (produto != null)
            {
                ReadProdutoDto produtoDto = _mapper.Map<ReadProdutoDto>(produto);
                return Ok(produtoDto);
            }
            return NotFound();
        }


    }
}
