using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using cardapio_digital.Dtos;
using cardapio_digital.Entities;

namespace cardapio_digital.Services
{
 public class MenuService
{
    private readonly AppDbContext _db;

    public MenuService(AppDbContext db)
    {
      _db = db;
    }

    public async Task<IResult> GetAll(int userId)
    {
        var school = await _db.Schools.FirstOrDefaultAsync(e => e.UserId == userId);
        if (school == null)
        {
            return Results.NotFound("School not found.");
        }

        var menus = await _db.Menus
        .Include(c => c.School)
        .Where(c => c.SchoolId == school.Id)
        .ToListAsync();

    var response = menus.Select(c => new MenuResponseDto
    {
        Id = c.Id,
        Name = c.Name,
        School = c.School
    }).ToList();

        return Results.Ok(response);
    }

    public async Task<IResult> Register(MenuDto dto, int userId)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            return Results.BadRequest("Name is required.");
        }

        var school = await _db.Schools.FirstOrDefaultAsync(e => e.UserId == userId);
        if (school == null)
        {
            return Results.NotFound("School not found.");
        }

        var menu = new Menu
        {
            Name = dto.Name,
            SchoolId = school.Id
        };

        _db.Menus.Add(menu);
        await _db.SaveChangesAsync();

        var menuWithSchool = await _db.Menus
        .Include(c => c.School)
        .FirstOrDefaultAsync(c => c.Id == menu.Id);

        if (menuWithSchool is null)
        {
            return Results.BadRequest("Error retrieving created menu.");
        }

        var response = new MenuResponseDto
        {
            Id = menuWithSchool.Id,
            Name = menuWithSchool.Name,
            School = menuWithSchool.School
        };

        return Results.Created($"/menus/{menu.Id}", response);
        }

        public async Task<IResult> AddProduct(int menuId, int productId)
        {
        var menu = await _db.Menus.FindAsync(menuId);
        if (menu == null)
        {
            return Results.BadRequest("Menu not found.");
        }

        var product = await _db.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == productId);
        if (product == null)
        {
            return Results.BadRequest("Product not found.");
        }

        if (product.SchoolId != menu.SchoolId)
        {
        return Results.BadRequest("Product belongs to another school.");
        }

        var exists = await _db.MenuProducts.AnyAsync(mp => mp.MenuId == menuId && mp.ProductId == productId);
        if (exists)
        {
            return Results.BadRequest("Product already exists in menu.");
        }

        var menuProduct = new MenuProduct
        {
            MenuId = menuId,
            ProductId = productId
        };

        _db.MenuProducts.Add(menuProduct);
        await _db.SaveChangesAsync();

         var response = new MenuProductResponseDto
        {
            MenuId = menuProduct.MenuId,
            Menu = menu.Name,
            ProductId = menuProduct.ProductId,
            Product = product.Name,
            Category = product.Category?.Name ?? "No Category"
        };

        return Results.Created($"/menu-products/{menuId}/{productId}", response);
        }
    }
}