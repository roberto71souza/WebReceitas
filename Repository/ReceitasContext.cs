using Dominio;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository
{
    public class ReceitasContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Receita> Receitas { get; set; }
        public ReceitasContext(DbContextOptions<ReceitasContext> opt) : base(opt)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Receita>()
                    .HasOne(b => b.Usuario)
                    .WithMany(a => a.Receitas)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
