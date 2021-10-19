using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SisProdutos
{
    public class Produto
    {
        public int Id { get; set; }
        public string nome { get; set; }
        public string descicao { get; set; }
        public float preco { get; set; }
        public string palavraChave { get; set; }
        public string categoria { get; set; }
    }
}
