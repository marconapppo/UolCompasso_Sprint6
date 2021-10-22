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
        public List<Produto> AjustarProdutoList(List<Produto> produtoList, string Nome ,
            string PalavraChave , string Descricao, string Categoria, bool? AscPreco , UserContext _context)
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

            if (Categoria != null)
            {
                produtoList = produtoList.Where(produto => produto.Categoria == Categoria).ToList();
            }

            if (AscPreco != null)
            {
                if(AscPreco == true) { produtoList = produtoList.OrderBy(produto => produto.Preco).ToList(); }
                if(AscPreco == false) { produtoList = produtoList.OrderByDescending(produto => produto.Preco).ToList(); }
            }
            return produtoList;
        }

        public List<Cidade> PegaCidadesProduto(UserContext _context, int idProduto)
        {
            var produtoCidadesList = _context.ProdutoCidades.Where(produto => produto.ProdutoId == idProduto).ToList();
            var cidadesList = new List<Cidade>();
            foreach (var produtoCidade in produtoCidadesList)
            {
                Cidade c = _context.Cidades.FirstOrDefault(cidade => cidade.Id == produtoCidade.CidadeId);
                cidadesList.Add(c);
            }
            return cidadesList;
        }

    }
}
