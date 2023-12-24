using Escolar32.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Escolar32.Context
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {}
        public DbSet<Aluno> Alunos { get; set; }
   
        public DbSet<Escola> Escolas { get; set; }

        public DbSet<Bairro> Bairros { get; set; }
                                
    }
}