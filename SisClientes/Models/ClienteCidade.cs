using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SisClientes
{
    public class ClienteCidade
    {

        public int Id { get; set; }

        [ForeignKey("Cidade")]
        public int CidadeId { get; set; }

        public Cidade Cidade { get; set; }

        [ForeignKey("Cliente")]
        public int ClienteId { get; set; }

        public Cliente cliente { get; set; }

        public bool Principal { get; set; }

    }
}
