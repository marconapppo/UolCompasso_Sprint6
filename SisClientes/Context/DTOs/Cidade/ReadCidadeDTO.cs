using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SisClientes
{
    public class ReadCidadeDTO
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Estado { get; set; }
    }
}
