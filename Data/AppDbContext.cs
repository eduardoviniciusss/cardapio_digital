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
       public DbSet<School> Schools => Set<School>();

       public DbSet<Menu> Menus => Set<Menu>();

       public DbSet<Category> Categories => Set<Category>();

       public DbSet<Product> Products => Set<Product>();

       public DbSet<MenuProduct> MenuProducts => Set<MenuProduct>();

       public DbSet<User> User => Set<User>();

       public DbSet<Parent> Parents => Set<Parent>();

       public DbSet<Child> Children => Set<Child>();

       public AppDbContext(DbContextOptions<AppDbContext>options):
       base(options){}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MenuProduct>()
            .HasKey(cp => new
            {
                cp.MenuId,
                cp.ProductId
            });

            // Configura o EF para salvar a List<Shift> como uma string JSON no banco
            modelBuilder.Entity<School>()
                .Property(e => e.Shifts)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null!),
                    v => JsonSerializer.Deserialize<List<Shift>>(v, (JsonSerializerOptions)null!) ?? new List<Shift>()
                );

            //Configuração de User
            modelBuilder.Entity<User>()
            .HasIndex(x => x.Email)
            .IsUnique();

            modelBuilder.Entity<User>()
            .Property(x => x.Name)
            .HasMaxLength(100);

            modelBuilder.Entity<User>()
            .Property(x => x.Email)
            .HasMaxLength(150);

            modelBuilder.Entity<User>()
            .Property(x => x.PasswordHash)
            .HasMaxLength(500);
            
            //Relacionamente 1:1 User e School
            modelBuilder.Entity<User>()
            .HasOne(u => u.School)
            .WithOne(e => e.User)
            .HasForeignKey<School>(e => e.UserId)
            .IsRequired();

            //Relacionamento 1:1 User e Parent"
            modelBuilder.Entity<Parent>()
            .HasOne(p => p.User)
            .WithOne(u => u.Parent)
            .HasForeignKey<Parent>(p => p.UserId)
            .IsRequired();

            //Relacionamento 1:N Parent e filho
            modelBuilder.Entity<Child>()
            .HasOne(f => f.Parent)
            .WithMany(p => p.Children)
            .HasForeignKey(f => f.ParentId)
            .IsRequired();

            //Relacionamento 1:N School e filho
            modelBuilder.Entity<Child>()
            .HasOne(f => f.School)
            .WithMany(e => e.Children)
            .HasForeignKey(f => f.SchoolId)
            .IsRequired();

            //Trocando int para string em perfil
             modelBuilder.Entity<User>()
            .Property(u => u.Role)
            .HasConversion<string>();

            //Prmintindo data de nascimento sem a hora
             modelBuilder.Entity<Child>()
             .Property(f => f.BirthDate)
             .HasColumnType("date");

            
        }

    }
}
