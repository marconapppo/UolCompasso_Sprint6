using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SisClientes
{
    public class CreateClienteDTO
    {

        public string Nome { get; set; }

        public DateTime DataNascimento { get; set; }

        public string Cep { get; set; }

        public string Logradouro { get; set; }

        public string Bairro { get; set; }

    }
}
