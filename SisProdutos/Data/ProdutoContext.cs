using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SisProdutos
{
    public class ProdutoContext : IdentityDbContext<IdentityUser<int>, IdentityRole<int>, int>
    {

        public ProdutoContext(DbContextOptions<ProdutoContext> opt) : base(opt)
        {

        }

    }
}
