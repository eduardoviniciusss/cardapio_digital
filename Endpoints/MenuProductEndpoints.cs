using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using cardapio_digital.Entities;
using cardapio_digital.Dtos;

namespace cardapio_digital.Endpoints
{
public static class MenuProductEndpoints
{

private static async Task<School?> GetMySchoolAsync(AppDbContext db, HttpContext http)
{
    var userId = int.Parse(http.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    return await db.Schools.FirstOrDefaultAsync(e => e.UserId == userId);
}

private static MenuProductResponseDto ToResponse(MenuProduct cp) => new()
{
    MenuId = cp.MenuId,
    Menu = cp.Menu.Name,
    ProductId = cp.ProductId,
    Product = cp.Product.Name,
    Category = cp.Product.Category?.Name ?? "No Category"
};

public static void MapMenuProductEndpoints(this WebApplication app)
{
// GET 
app.MapGet("/menu-products", async (AppDbContext db, HttpContext http) =>
{
    var school = await GetMySchoolAsync(db, http);
    if (school == null)
        return Results.NotFound("School not found.");

    var menuProducts = await db.MenuProducts
        .Include(cp => cp.Menu)
        .Include(cp => cp.Product).ThenInclude(p => p.Category)
        .Where(cp => cp.Menu.SchoolId == school.Id)
        .ToListAsync();

    return Results.Ok(menuProducts.Select(ToResponse));
})
.RequireAuthorization("Canteen");

// GET ID 
app.MapGet("/menu-products/{menuId}/{productId}", async (int menuId, int productId, AppDbContext db, HttpContext http) =>
{
    var school = await GetMySchoolAsync(db, http);
    if (school == null)
        return Results.NotFound("School not found.");

    var menuProduct = await db.MenuProducts
        .Include(cp => cp.Menu)
        .Include(cp => cp.Product).ThenInclude(p => p.Category)
        .FirstOrDefaultAsync(cp => cp.MenuId == menuId && cp.ProductId == productId && cp.Menu.SchoolId == school.Id);

    if (menuProduct is null)
        return Results.NotFound("Link not found.");

    return Results.Ok(ToResponse(menuProduct));
})
.RequireAuthorization("Canteen");

// POST
app.MapPost("/menu-products", async (AppDbContext db, HttpContext http, MenuProductDto dto) =>
{
    var school = await GetMySchoolAsync(db, http);
    if (school == null)
        return Results.NotFound("School not found.");

    var menu = await db.Menus.FirstOrDefaultAsync(m => m.Id == dto.MenuId && m.SchoolId == school.Id);
    if (menu is null)
        return Results.BadRequest("Menu not found.");

    var product = await db.Products.Include(p => p.Category)
        .FirstOrDefaultAsync(p => p.Id == dto.ProductId && p.SchoolId == school.Id);
    if (product is null)
        return Results.BadRequest("Product not found.");

    var alreadyLinked = await db.MenuProducts.AnyAsync(cp => cp.MenuId == dto.MenuId && cp.ProductId == dto.ProductId);
    if (alreadyLinked)
        return Results.BadRequest("This product is already in the menu.");

    var menuProduct = new MenuProduct
    {
        MenuId = menu.Id,
        ProductId = product.Id
    };
    db.MenuProducts.Add(menuProduct);
    await db.SaveChangesAsync();

    var response = new MenuProductResponseDto
    {
        MenuId = menuProduct.MenuId,
        Menu = menu.Name,
        ProductId = menuProduct.ProductId,
        Product = product.Name,
        Category = product.Category?.Name ?? "No Category"
    };

    return Results.Created($"/menu-products/{dto.MenuId}/{dto.ProductId}", response);
})
.RequireAuthorization("Canteen");

// PUT 
app.MapPut("/menu-products/{menuId}/{productId}", async (int menuId, int productId, AppDbContext db, HttpContext http, MenuProductDto dto) =>
{
    var school = await GetMySchoolAsync(db, http);
    if (school == null)
        return Results.NotFound("School not found.");

    var menuProduct = await db.MenuProducts
        .FirstOrDefaultAsync(cp => cp.MenuId == menuId && cp.ProductId == productId && cp.Menu.SchoolId == school.Id);
    if (menuProduct is null)
        return Results.NotFound("Link not found.");

    return await ReplaceLinkAsync(db, school, menuProduct, dto.MenuId, dto.ProductId);
})
.RequireAuthorization("Canteen");

// PATCH 
app.MapPatch("/menu-products/{menuId}/{productId}", async (int menuId, int productId, AppDbContext db, HttpContext http, MenuProductDto dto) =>
{
    var school = await GetMySchoolAsync(db, http);
    if (school == null)
        return Results.NotFound("School not found.");

    var menuProduct = await db.MenuProducts
        .FirstOrDefaultAsync(cp => cp.MenuId == menuId && cp.ProductId == productId && cp.Menu.SchoolId == school.Id);
    if (menuProduct is null)
        return Results.NotFound("Link not found.");

    var newMenuId = dto.MenuId > 0 ? dto.MenuId : menuProduct.MenuId;
    var newProductId = dto.ProductId > 0 ? dto.ProductId : menuProduct.ProductId;

    return await ReplaceLinkAsync(db, school, menuProduct, newMenuId, newProductId);
})
.RequireAuthorization("Canteen");

// DELETE 
app.MapDelete("/menu-products/{menuId}/{productId}", async (int menuId, int productId, AppDbContext db, HttpContext http) =>
{
    var school = await GetMySchoolAsync(db, http);
    if (school == null)
        return Results.NotFound("School not found.");

    var menuProduct = await db.MenuProducts
        .FirstOrDefaultAsync(cp => cp.MenuId == menuId && cp.ProductId == productId && cp.Menu.SchoolId == school.Id);
    if (menuProduct is null)
        return Results.NotFound("Link not found.");

    db.MenuProducts.Remove(menuProduct);
    await db.SaveChangesAsync();
    return Results.NoContent();
})
.RequireAuthorization("Canteen");
}

private static async Task<IResult> ReplaceLinkAsync(
    AppDbContext db, School school, MenuProduct current, int newMenuId, int newProductId)
{
    var menu = await db.Menus.FirstOrDefaultAsync(m => m.Id == newMenuId && m.SchoolId == school.Id);
    if (menu is null)
        return Results.BadRequest("Menu not found.");

    var product = await db.Products.Include(p => p.Category)
        .FirstOrDefaultAsync(p => p.Id == newProductId && p.SchoolId == school.Id);
    if (product is null)
        return Results.BadRequest("Product not found.");

    var response = new MenuProductResponseDto
    {
        MenuId = menu.Id,
        Menu = menu.Name,
        ProductId = product.Id,
        Product = product.Name,
        Category = product.Category?.Name ?? "No Category"
    };

    if (current.MenuId == newMenuId && current.ProductId == newProductId)
        return Results.Ok(response);

    var alreadyLinked = await db.MenuProducts.AnyAsync(cp => cp.MenuId == newMenuId && cp.ProductId == newProductId);
    if (alreadyLinked)
        return Results.BadRequest("This product is already in the menu.");

    db.MenuProducts.Remove(current);
    db.MenuProducts.Add(new MenuProduct { MenuId = newMenuId, ProductId = newProductId });
    await db.SaveChangesAsync();

    return Results.Ok(response);
}
}
}
