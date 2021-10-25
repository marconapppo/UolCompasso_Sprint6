using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SisProdutos
{
    [BsonIgnoreExtraElements]
    public class ClienteProduto
    {
        public ClienteProduto(int idCliente, int idProduto, List<string> errosList)
        {
            IdCliente = idCliente;
            IdProduto = idProduto;
            ErrosList = errosList;
        }

        public int IdCliente { get; set; }
        public int IdProduto { get; set; }
        public List<string> ErrosList { get; set; }

    }
}
