using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SisProdutos
{
    public class CreateClienteDto
    {
        [Required]
        public string Nome { get; set; }
        [Required]
        public DateTime DataNascimento { get; set; }
        [Required]
        public string Cep { get; set; }
        [Required]
        public string Logradouro { get; set; }
        [Required]
        public string Bairro { get; set; }

    }
}
