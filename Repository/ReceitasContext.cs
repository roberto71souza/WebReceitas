using Dominio;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
namespace Repository
{
    public class ReceitasContext : IdentityDbContext<Usuario, IdentityRole<int>, int>
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Receita> Receitas { get; set; }
        public ReceitasContext(DbContextOptions<ReceitasContext> opt) : base(opt)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Receita>().Property(r => r.Titulo).HasMaxLength(80);
            modelBuilder.Entity<Receita>().Property(r => r.Acessório).HasMaxLength(150);
            modelBuilder.Entity<Receita>().Property(r => r.Data_Publicacao).HasMaxLength(20);

            modelBuilder.Entity<Usuario>().Property(r => r.Nome).HasMaxLength(60);
            modelBuilder.Entity<Usuario>().Property(r => r.UserName).HasMaxLength(60);
            modelBuilder.Entity<Usuario>().Property(r => r.Email).HasMaxLength(60);
            modelBuilder.Entity<Usuario>().Property(r => r.Cidade).HasMaxLength(20);
            modelBuilder.Entity<Usuario>().Property(r => r.Estado).HasMaxLength(20);
            modelBuilder.Entity<Usuario>().Property(r => r.Data_Nascimento).HasMaxLength(20);

            modelBuilder.Entity<Receita>()
                    .HasOne(b => b.Usuario)
                    .WithMany(a => a.Receitas)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

            /*modelBuilder.Entity<Usuario>(key =>
            {
                key.ToTable("Usuario");
                key.HasKey(x => x.Id);

                key.HasMany<Receita>()
                .WithOne()
                .HasForeignKey(x => x.Usuario)
                .IsRequired(false);
            });*/
        }
    }
}
