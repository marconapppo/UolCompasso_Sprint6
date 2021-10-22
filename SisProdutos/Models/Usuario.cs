using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SisProdutos
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Username { get; set; }
    }
}
