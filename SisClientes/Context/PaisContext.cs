using Microsoft.EntityFrameworkCore;

using System;

namespace SisClientes
{
    public class PaisContext : DbContext
    {
        
        public DbSet<Cidade> Cidades { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<ClienteCidade> ClienteCidade { get; set; }

        public PaisContext(DbContextOptions<PaisContext> opt) : base(opt)
        {

        }
    }
}
