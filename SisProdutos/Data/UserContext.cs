using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SisProdutos
{
    public class UserContext : IdentityDbContext<IdentityUser<int>, IdentityRole<int>, int>
    {

        public UserContext(DbContextOptions<UserContext> opt) : base(opt)
        {

        }

        public DbSet<Produto> Produtos { get; set; }
    }
}
