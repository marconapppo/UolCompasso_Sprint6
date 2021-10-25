using AutoMapper;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;


namespace SisClientes
{
    
    public class ClienteControllerFunctions
    {
        public async Task<CatalogCep> GetCepPeloSiteAsync(string cep, string logradouro, string bairro, HttpClient _httpClient)
        {
            var responseString = await _httpClient.GetStringAsync("https://viacep.com.br/ws/" + cep + "/json/");
            var catalog = JsonConvert.DeserializeObject<CatalogCep>(responseString);
            //recebendo valores de cidade e catalog, e caso logradouro ou bairro seja null, insere o que veio no corpo
            if (!string.IsNullOrEmpty(catalog.logradouro)) { logradouro = catalog.logradouro; }
            if (!string.IsNullOrEmpty(catalog.bairro)) { bairro = catalog.bairro; }
            return catalog;
        }
        public async Task InserindoAssociativaCLienteCidadeAsync(int clienteId, int cidadeIdPrincipal, List<CidadeCep> CepOpcionais, PaisContext _context, HttpClient _httpClient)
        {
            //inserindo no banco a cidade principal
            ClienteCidade clienteCidadePrincipal = new ClienteCidade();
            clienteCidadePrincipal.ClienteId = clienteId;
            clienteCidadePrincipal.CidadeId = cidadeIdPrincipal;
            clienteCidadePrincipal.Principal = true;
            _context.ClienteCidade.Add(clienteCidadePrincipal);
            _context.SaveChanges();
            //pega o nome das cidades pelos CEPs e colocar numa lista de cidades
            List<ClienteCidade> clienteCidadeList = new List<ClienteCidade>();
            foreach (var cidadeCep in CepOpcionais)
            {
                //pega o nome das cidade
                CatalogCep catalog = new CatalogCep();
                catalog = await GetCepPeloSiteAsync(cidadeCep.Cep, cidadeCep.Logradouro, cidadeCep.Bairro, _httpClient);
                //pega o id da cidade
                int cidadeId = _context.Cidades.FirstOrDefault(cidade => cidade.Nome == catalog.localidade).Id;
                //Cria um cliente Cidade
                ClienteCidade clienteCidade = new ClienteCidade();
                clienteCidade.ClienteId = clienteId;
                clienteCidade.CidadeId = cidadeId;
                clienteCidade.Principal = false;
                clienteCidadeList.Add(clienteCidade);
            }
            //insere no banco
            _context.ClienteCidade.AddRange(clienteCidadeList);
            _context.SaveChanges();
        }

    }
        
}
