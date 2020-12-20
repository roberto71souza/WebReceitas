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

/*            modelBuilder.Entity<Usuario>(key =>
            {
                key.ToTable("Usuario");
                key.HasKey(x => x.Id);

                key.HasMany<Receita>()
                .WithOne()
                .HasForeignKey(x => x.Usuario)
                .IsRequired(false);
            });*/

                 modelBuilder.Entity<Receita>()
                    .HasOne(b => b.Usuario)
                    .WithMany(a => a.Receitas)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
