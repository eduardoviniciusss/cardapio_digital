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

       public AppDbContext(DbContextOptions<AppDbContext>options):
       base(options){}
    }
}