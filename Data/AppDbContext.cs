using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using cardapio_digital.Enums;
using cardapio_digital.Entities;

namespace cardapio_digital
{
    public class AppDbContext : DbContext
    {
       public DbSet<Escola> Escolas => Set<Escola>();

       public DbSet<Cardapio> Cardapios => Set<Cardapio>();

       public DbSet<Categoria> Categorias => Set<Categoria>();

       public DbSet<Produto> Produtos => Set<Produto>();

       public DbSet<CardapioProduto> CardapioProdutos => Set<CardapioProduto>();

       public DbSet<Usuario> Usuarios => Set<Usuario>();

       public DbSet<Pais> Pais => Set<Pais>();

       public DbSet<Filho> Filho => Set<Filho>();

       public AppDbContext(DbContextOptions<AppDbContext>options):
       base(options){}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CardapioProduto>()
            .HasKey(cp => new
            {
                cp.CardapioId,
                cp.ProdutoId
            });

            // Configura o EF para salvar a List<Turno> como uma string JSON no banco
            modelBuilder.Entity<Escola>()
                .Property(e => e.Turnos)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null!),
                    v => JsonSerializer.Deserialize<List<Turno>>(v, (JsonSerializerOptions)null!) ?? new List<Turno>()
                );

            //Configuração de Usuario
            modelBuilder.Entity<Usuario>()
            .HasIndex(x => x.Email)
            .IsUnique();

            modelBuilder.Entity<Usuario>()
            .Property(x => x.Nome)
            .HasMaxLength(100);

            modelBuilder.Entity<Usuario>()
            .Property(x => x.Email)
            .HasMaxLength(150);

            modelBuilder.Entity<Usuario>()
            .Property(x => x.SenhaHash)
            .HasMaxLength(500);
            
            //Relacionamente 1:1 Usuario e Escola
            modelBuilder.Entity<Usuario>()
            .HasOne(u => u.Escola)
            .WithOne(e => e.Usuario)
            .HasForeignKey<Escola>(e => e.UsuarioId)
            .IsRequired();

            //Relacionamento 1:1 Usuario e Pais
            modelBuilder.Entity<Pais>()
            .HasOne(p => p.Usuario)
            .WithOne(u => u.Pais)
            .HasForeignKey<Pais>(p => p.UsuarioId)
            .IsRequired();

            //Relacionamento 1:N Pais e filho
            modelBuilder.Entity<Filho>()
            .HasOne(f => f.Pais)
            .WithMany(p => p.Filhos)
            .HasForeignKey(f => f.PaisId)
            .IsRequired();

            //Relacionamento 1:N Escola e filho
            modelBuilder.Entity<Filho>()
            .HasOne(f => f.Escola)
            .WithMany(e => e.Filhos)
            .HasForeignKey(f => f.EscolaId)
            .IsRequired();


           

            //Trocando int para string em perfil
             modelBuilder.Entity<Usuario>()
            .Property(u => u.Perfil)
            .HasConversion<string>();

            

            
        }

    }
}