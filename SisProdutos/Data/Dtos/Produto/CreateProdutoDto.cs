﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SisProdutos
{
    public class CreateProdutoDto
    {
        [Required]
        public string Nome { get; set; }
        [Required]
        public string Descicao { get; set; }
        [Required]
        public decimal Preco { get; set; }
        [Required]
        public string PalavraChave { get; set; }
        [Required]
        public string Categoria { get; set; }
        [Required]
        public int CidadeId { get; set; }
    }
}
