using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        }
    }
}