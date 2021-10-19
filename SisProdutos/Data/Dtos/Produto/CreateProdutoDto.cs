using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SisProdutos
{
    public class CreateProdutoDto
    {
        [Required]
        public string nome { get; set; }
        [Required]
        public string descicao { get; set; }
        [Required]
        public decimal preco { get; set; }
        [Required]
        public string palavraChave { get; set; }
        [Required]
        public string categoria { get; set; }


    }
}
