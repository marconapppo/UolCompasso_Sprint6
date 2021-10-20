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
    
    public class ProdutoControllerFunctions
    {
        public List<Produto> AjustarProdutoList(List<Produto> produtoList, string Nome , string PalavraChave , string Descricao , UserContext _context)
        {
            if (Nome != null)
            {
                produtoList = _context.Produtos.Where(produto => produto.Nome == Nome).ToList();
            }

            if (PalavraChave != null)
            {
                produtoList = produtoList.Where(produto => produto.PalavraChave == PalavraChave).ToList();
            }

            if (Descricao != null)
            {
                produtoList = produtoList.Where(produto => produto.Descricao == Descricao).ToList();
            }
            return produtoList;
        }

    }
}
