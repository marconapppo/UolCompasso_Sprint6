using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SisProdutos
{
    public class ReadProdutoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descicao { get; set; }
        public float Preco { get; set; }
        public string PalavraChave { get; set; }
        public string Categoria { get; set; }
        public int CidadeId { get; set; }
    }
}
