using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using cardapio_digital.Entities;
using cardapio_digital.Dtos;

namespace cardapio_digital.Endpoints
{
public static class MenuProductEndpoints
{
public static void MapMenuProductEndpoints(this WebApplication app)
{
// GET CARDAPIO_PRODUTO
app.MapGet("/menu-products", async (AppDbContext db) =>
{
    var menuProducts = await db.MenuProducts
    .Include(cp => cp.Menu).ThenInclude(c => c.School)
    .Include(cp => cp.Product).ThenInclude(p => p.Category)
    .ToListAsync();
var response = menuProducts.Select(cardapioProduto => new MenuProductResponseDto
 {
   MenuId = cardapioProduto.MenuId,
   Menu = cardapioProduto.Menu.Name,
   ProductId = cardapioProduto.ProductId,
   Product = cardapioProduto.Product.Name,
   Category = cardapioProduto.Product.Category.Name
});
return Results.Ok(response);
});

// GET ID CARDAPIO_PRODUTO
app.MapGet("/menu-products/{menuId}/{productId}",async (int menuId, int productId, AppDbContext db) =>
{
    var menuProducts = await db.MenuProducts
    .Include(cp => cp.Menu).ThenInclude(c => c.School)
    .Include(cp => cp.Product).ThenInclude(p => p.Category)
    .FirstOrDefaultAsync(cp => cp.MenuId == menuId && cp.ProductId == productId);
    if (menuProducts is null)
    {
        return Results.NotFound("Link not found.");
    }
    var response = new MenuProductResponseDto
    {
        MenuId = menuProducts.MenuId,
        Menu = menuProducts.Menu.Name,

        ProductId = menuProducts.ProductId,
        Product = menuProducts.Product.Name,

        Category = menuProducts.Product.Category.Name
    };

    return Results.Ok(response);
});

// POST CARDAPIO_PRODUTO
app.MapPost("/menu-products",async (AppDbContext db, MenuProductDto dto) =>
{
    var menuExists = await db.Menus.FindAsync(dto.MenuId);
    if (menuExists is null)
    {
        return Results.BadRequest("Menu not found.");
    }
    var productExists = await db.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == dto.ProductId);
    if (productExists is null)
    {
        return Results.BadRequest("Product not found.");
    }
    var thereIsAConnection = await db.MenuProducts.AnyAsync
    (cp => cp.MenuId == dto.MenuId && cp.ProductId == dto.ProductId);
    if (thereIsAConnection)
    {
        return Results.BadRequest("This product is already in the menu.");
    }
    var menuProduct = new MenuProduct
    {
        MenuId = dto.MenuId,
        ProductId = dto.ProductId
    };
    db.MenuProducts.Add(menuProduct); 
    await db.SaveChangesAsync();
     var response = new MenuProductResponseDto
    {
        MenuId = menuProduct.MenuId,
        Menu = menuExists.Name,

        ProductId = menuProduct.ProductId,
        Product = productExists.Name,

        Category = productExists.Category?.Name ?? "No Category"
    };

    return Results.Created($"/menu-products/{dto.MenuId}/{dto.ProductId}", response);
});

// PUT CARDAPIO_PRODUTO
app.MapPut("/menu-products/{menuId}/{productId}", async (int menuId,int productId,AppDbContext db,MenuProductDto dto) =>
{
    var menuProduct = await db.MenuProducts.FirstOrDefaultAsync(cp => cp.MenuId == menuId && 
    cp.ProductId == productId);
    if (menuProduct is null)
    {
         return Results.NotFound("Link not found.");
    }
    var menuExists = await db.Menus.FindAsync(dto.MenuId);
    if (menuExists is null)
    {
        return Results.BadRequest("Menu not found.");
    }
    var productExists = await db.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == dto.ProductId);
    if (productExists is null)
    {
        return Results.BadRequest("Product not found.");
    }
    db.MenuProducts.Remove(menuProduct);
    var newRecord = new MenuProduct
    {
        MenuId = dto.MenuId,
        ProductId = dto.ProductId
    };
    db.MenuProducts.Add(newRecord);
    await db.SaveChangesAsync();
    var response = new MenuProductResponseDto
    {
        MenuId = newRecord.MenuId,
        Menu = menuExists.Name,

        ProductId = newRecord.ProductId,
        Product = productExists.Name,

        Category = productExists.Category?.Name ?? "No Category"
    };

    return Results.Ok(response);
});

// PATCH CARDAPIO_PRODUTO
app.MapPatch("/menu-products/{menuId}/{productId}", async (int menuId, int productId, AppDbContext db, MenuProductDto dto) =>
    {
    var menuProduct = await db.MenuProducts.FirstOrDefaultAsync
    (cp => cp.MenuId == menuId && cp.ProductId == productId);
    if (menuProduct is null)
    {
        return Results.NotFound("Link not found.");
    }
    if (dto.MenuId > 0)
    {
        var menuExists = await db.Menus.FindAsync(dto.MenuId);
        if (menuExists is null)
        {
            return Results.BadRequest("Menu not found.");
        }
        menuProduct.MenuId = dto.MenuId;
    }
    if (dto.ProductId > 0)
    {
        var productExists = await db.Products.FindAsync(dto.ProductId);
        if (productExists is null)
        {
            return Results.BadRequest("Product not found.");
        }
        menuProduct.ProductId = dto.ProductId;
    }
    await db.SaveChangesAsync();

    var updatedProductMenu  = await db.MenuProducts
        .Include(cp => cp.Menu)
        .Include(cp => cp.Product).ThenInclude(p => p.Category)
        .FirstOrDefaultAsync(cp => cp.MenuId == menuProduct.MenuId && cp.ProductId == menuProduct.ProductId);

    if (updatedProductMenu  is null) return Results.BadRequest("Error retrieving updated link.");

    var response = new MenuProductResponseDto
    {
        MenuId = updatedProductMenu.MenuId,
        Menu = updatedProductMenu.Menu.Name,
        ProductId = updatedProductMenu.ProductId,
        Product = updatedProductMenu.Product.Name,
        Category = updatedProductMenu.Product.Category?.Name ?? "No Category"
    };
    return Results.Ok(response);
});

// DELETE CARDAPIO_PRODUTO
app.MapDelete("/menu-products/{menuId}/{productId}",async (int menuId, int productId, AppDbContext db) =>
{
    var menuProducts = await db.MenuProducts.FirstOrDefaultAsync
    (cp => cp.MenuId == menuId && cp.ProductId == productId);
    if (menuProducts is null)
    {
        return Results.NotFound("Link not found.");
    }
    db.MenuProducts.Remove(menuProducts);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

        }
    }
}
