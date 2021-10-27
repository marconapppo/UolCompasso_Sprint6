using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SisProdutos
{
    public class CreateProdutoDto
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public float Preco { get; set; }
        public string PalavraChave { get; set; }
        public string Categoria { get; set; }
        public List<string> CepOpcionais { get; set; }
    }
}
