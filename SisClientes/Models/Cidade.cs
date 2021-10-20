using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SisClientes
{
    public class Cidade
    {

        public int Id { get; set; }
        public string Nome { get; set; }
        public string Estado { get; set; }
        public List<Cliente> Clientes { get; set; }
        //public List<ProdutoCidade> ProdutoCidades { get; set; }
    }
}
