using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SisProdutos
{
    public class Usuario
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Username { get; set; }
    }
}
