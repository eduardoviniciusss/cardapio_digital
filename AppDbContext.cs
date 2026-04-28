using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cardapio_digital.Entities;
using Microsoft.EntityFrameworkCore;

namespace cardapio_digital
{
    public class AppDbContext : DbContext
  {
    public DbSet<Produto> Produtos => Set<Produto>();

    public AppDbContext(DbContextOptions<AppDbContext>options):
    base(options){}
}
}