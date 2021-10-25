using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auditoria
{
    [BsonIgnoreExtraElements]
    public class ClienteProduto
    {
        public int IdCliente { get; set; }
        public int IdProduto { get; set; }
        public List<string> ErrosList { get; set; }

    }
}
