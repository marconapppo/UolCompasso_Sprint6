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
            return Ok();
        }
    }
}
