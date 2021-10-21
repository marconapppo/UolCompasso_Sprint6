using System;
using System.ComponentModel.DataAnnotations;

namespace SisClientes
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public int CidadeId { get; set; }
        public string Cep { get; set; }
        public string Logradouro { get; set; }
        public string Bairro { get; set; }
        public Cidade Cidade { get; set; }
    }
}
