using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auditoria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuditoriaController : ControllerBase
    {
        MongoDB mongo = new MongoDB();

        [HttpPost]
        public void PostPedido([FromBody] ClienteProduto clienteProduto)
        {
            //conectando no mongo
            mongo.ConectandoBanco();
            //inserindo no mongo
            mongo.InserindoBanco(clienteProduto);
        }

        [HttpGet]
        public string GetPedido()
        {
            //conectando no banco
            mongo.ConectandoBanco();
            //retornando os valores de Pedido
            var clienteProduto = mongo.retornandoBanco();

            //adicionando no array items
            var docArray = new BsonArray(); 
            foreach (var row in clienteProduto)
            {
                //docArray.Add(BsonDocument.Parse(pedidos.ToJson<Pedido>()));
                docArray.Add(BsonDocument.Parse(row.ToJson<ClienteProduto>()));
            }
            return docArray.ToJson();
        }

        


    }
}
