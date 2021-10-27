using AutoMapper;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SisProdutos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
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

        public async Task<List<int>> GetListCidadeIdsAsync(List<string> cepList, HttpClient _httpClient, UserContext _context)
        {
            List<int> cidadesIds = new List<int>();
            //pegando o localidade usando o cep, pelo site
            foreach(var cep in cepList)
            {
                var responseString = await _httpClient.GetStringAsync("https://viacep.com.br/ws/" + cep.Replace("-", "") + "/json/");
                var catalog = JsonConvert.DeserializeObject<CatalogCep>(responseString);
                //pegando id da cidade
                Cidade cidade = _context.Cidades.FirstOrDefault(cidade => cidade.Nome == catalog.localidade);
                int cidadeId = cidade.Id;
                cidadesIds.Add(cidadeId);
            }
            //pega os Ids pelo banco
            return cidadesIds;
        }
        
        public async Task<int> GetIdPrincipalClienteAsync(int idCliente, HttpClient _httpClient)
        {
            //pega o cep do cliente
            var responseString = await _httpClient.GetStringAsync("https://localhost:5001/api/Cliente/" + idCliente);
            var cliente = JsonConvert.DeserializeObject<Cliente>(responseString);
            //pegar id do cep do cliente
            responseString = await _httpClient.GetStringAsync("https://localhost:5001/api/Cidade/cep/" + cliente.Cep);
            var cidade = JsonConvert.DeserializeObject<Cidade>(responseString);
            return cidade.Id;
        }

        public async Task<int> GetIdClienteAsync(IdentityUser<int> usuario, HttpClient _httpClient)
        {
            //pega todos os cliente
            var responseString = await _httpClient.GetStringAsync("https://localhost:5001/api/Cliente");
            var clienteList = JsonConvert.DeserializeObject<List<Cliente>>(responseString);
            //confere qual cliente é o nosso cliente
            Cliente clienteAchado = new Cliente();
            foreach(var cliente in clienteList)
            {
                if (cliente.Id == usuario.Id)
                {
                    clienteAchado = cliente;
                }
            }
            if(clienteAchado == null) { throw new ArgumentNullException(nameof(clienteAchado)); }
            return clienteAchado.Id;
        }

        public IdentityUser<int> GetUsuario(string nomeUsuario, UserContext _context)
        {
            var usuario = _context.Users.FirstOrDefault(user => user.UserName == nomeUsuario);
            return usuario;
        }

        public float GetPrecoProduto(int idProduto,UserContext _context)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.Id == idProduto);
            if (produto == null) { throw new ArgumentNullException(nameof(produto)); }
            var precoProduto = produto.Preco;
            precoProduto = (float)Math.Round(precoProduto);
            return precoProduto;
        }

        public async Task<Cidade> GetCidadeClienteAsync(int idCidadeCliente,HttpClient _httpClient)
        {
            var responseString = await _httpClient.GetStringAsync("https://localhost:5001/api/Cidade/" + idCidadeCliente);
            var cidadeCliente = JsonConvert.DeserializeObject<Cidade>(responseString);
            if(cidadeCliente == null) { throw new ArgumentNullException(nameof(cidadeCliente)); }
            return cidadeCliente;
        }

        public string GetRespostaFrete(List<Cidade> cidadesList, string cidadeClienteNome, float precoProduto)
        {
            string resposta = "Frete de R$ 29,90, Valor:" + (precoProduto + 29.90).ToString("n2");
            foreach (var cidadeProduto in cidadesList)
            {
                if (cidadeProduto.Nome == cidadeClienteNome) { resposta = "Sem frete, Valor:" + precoProduto.ToString("n2"); }
            }
            return resposta;
        }
    }
}
