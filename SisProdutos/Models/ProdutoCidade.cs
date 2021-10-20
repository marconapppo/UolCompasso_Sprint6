using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SisProdutos
{
    public class ProdutoCidade
    {
        public int Id { get; set; }
        [ForeignKey("Cidade")]
        public int CidadeId { get; set; }
        public Cidade Cidade { get; set; }
        [ForeignKey("Produto")]
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; }

    }
}
