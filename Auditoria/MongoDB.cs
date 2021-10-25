using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auditoria
{
    public class MongoDB
    {
        IMongoCollection<ClienteProduto> _colecao;

        public void ConectandoBanco()
        {
            string stringConexao = "mongodb://localhost:27017/Admin";
            IMongoClient cliente = new MongoClient(stringConexao);

            IMongoDatabase db = cliente.GetDatabase("Auditoria");

            _colecao = db.GetCollection<ClienteProduto>("UsuarioProduto");
        }

        public void InserindoBanco(ClienteProduto clienteProduto)
        {

            _colecao.InsertOne(clienteProduto);

            
        }

        public List<ClienteProduto> retornandoBanco()
        {
            var bsonDocument = _colecao.Find(new BsonDocument()).ToList();
            return bsonDocument;
        }
    }
}
