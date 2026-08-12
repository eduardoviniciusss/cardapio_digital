using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using cardapio_digital.Entities;
using cardapio_digital.Dtos;
using System.Security.Claims;

namespace cardapio_digital.Endpoints
{
public static class ProductEndpoints
{
public static void MapProductEndpoints(this WebApplication app)
{
//GET PRODUTO
app.MapGet("/products",async (AppDbContext db, HttpContext http) =>
{
    var userId = int.Parse(http.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    var school = await db.Schools.FirstOrDefaultAsync(e => e.UserId == userId);

if (school == null)
{
    return Results.NotFound("School not found.");
}
    var products = await db.Products .Include(p => p.Category).Where(p => p.SchoolId == school.Id).ToListAsync();
    var response = products.Select(p => new ProductResponseDto
    {
        Id = p.Id,
        Name = p.Name,
        Price = p.Price,
        Category = p.Category
    }).ToList();
    return Results.Ok(response);
})
.RequireAuthorization("Canteen");

//GET ID
app.MapGet("/products/{id}", async(int id, AppDbContext db, HttpContext http) =>
{
    var userId = int.Parse(http.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    var school = await db.Schools.FirstOrDefaultAsync(e => e.UserId == userId);

    if (school == null)
    {
        return Results.NotFound("School not found.");
    }

     var product = await db.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id && p.SchoolId == school.Id);
    if (product == null)
    {
        return Results.NotFound("Product not found.");
    }
    
    var response = new ProductResponseDto
    {
        Id = product.Id,
        Name = product.Name,
        Price = product.Price,
        Category = product.Category
    };
    return Results.Ok(response);
 })
 .RequireAuthorization("Canteen");

//POST PRODUTO
app.MapPost("/products", async (AppDbContext db, ProductDto dto, HttpContext http) =>
 {
    var userId = int.Parse(http.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    var school = await db.Schools.FirstOrDefaultAsync(e => e.UserId == userId);

     if (string.IsNullOrWhiteSpace(dto.Name))
    {
        return Results.BadRequest("Name is required.");
    }
    //verifica se a categoria existe
    var categoryExists = await db.Categories.FindAsync(dto.CategoryId);
    if (categoryExists is null)
    {
        return Results.BadRequest("Category not found.");
    }
    if (school == null)
    {
    return Results.NotFound("School not found.");
    }
    var product = new Product
    {
        Name = dto.Name,
        Price = dto.Price,
        CategoryId = dto.CategoryId,
        SchoolId = school.Id
    
    };
    db.Products.Add(product);
    await db.SaveChangesAsync();
    var productWithCategory = await db.Products
        .Include(p => p.Category)
        .FirstOrDefaultAsync(p => p.Id == product.Id);
    if (productWithCategory is null)
    {
        return Results.BadRequest("Erro retrieving the created product.");
    }
    var response = new ProductResponseDto
    {
        Id = productWithCategory.Id,
        Name = productWithCategory.Name,
        Price = productWithCategory.Price,
        Category = productWithCategory.Category
    };
    return Results.Created($"/products/{product.Id}", response);
 })
 .RequireAuthorization("Canteen");

//PUT PRODUTO
app.MapPut("/products/{id}", async (int id, AppDbContext db, ProductDto dto, HttpContext http) =>
 {
   var userId = int.Parse(http.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

   var school = await db.Schools.FirstOrDefaultAsync(e => e.UserId == userId);

if (school == null)
{
    return Results.NotFound("School not found.");
}

var product = await db.Products.FirstOrDefaultAsync(p => p.Id == id && p.SchoolId == school.Id);

    if (product is null)
    {
        return Results.NotFound("Product not found.");
    }

    if (string.IsNullOrWhiteSpace(dto.Name))
    {
        return Results.BadRequest("Name is required.");
    }

    var categoryExists = await db.Categories.FindAsync(dto.CategoryId);

    if (categoryExists is null)
    {
        return Results.BadRequest("Category not found.");
    }

    product.Name = dto.Name;
    product.Price = dto.Price;
    product.CategoryId = dto.CategoryId;
    

    await db.SaveChangesAsync();

    var productWithCategory = await db.Products
        .Include(p => p.Category)
        .FirstOrDefaultAsync(p => p.Id == product.Id);
    if (productWithCategory is null)
    {
        return Results.BadRequest("Erro retrieving the updated product.");
    }
    var response = new ProductResponseDto
    {
        Id = productWithCategory.Id,
        Name = productWithCategory.Name,
        Price = productWithCategory.Price,
        Category = productWithCategory.Category
    };
    return Results.Ok(response);
})
.RequireAuthorization("Canteen");



// PATCH PRODUTO
app.MapPatch("/products/{id}", async (int id, AppDbContext db, ProductDto dto, HttpContext http) =>
{
    var userId = int.Parse( http.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

var school = await db.Schools.FirstOrDefaultAsync(e => e.UserId == userId);

if (school == null)
{
    return Results.NotFound("School not found.");
}

var product = await db.Products.FirstOrDefaultAsync(p => p.Id == id && p.SchoolId == school.Id);
    if (product is null)
    {
      return Results.NotFound("Product not found.");
    }
    if (!string.IsNullOrWhiteSpace(dto.Name))
    {
       product.Name = dto.Name;
    }
    if (dto.CategoryId > 0)
    {
        var categoryExists = await db.Categories.FindAsync(dto.CategoryId);
        if (categoryExists is null)
        {
           return Results.BadRequest("Category not found.");
        }
        product.CategoryId = dto.CategoryId;
    }
    if (dto.Price > 0)
    {
        product.Price = dto.Price;
    }
    await db.SaveChangesAsync();
    var productWithCategory = await db.Products
        .Include(p => p.Category)
        .FirstOrDefaultAsync(p => p.Id == product.Id);
    if (productWithCategory is null)
    {
        return Results.BadRequest("Erro retrieving the updated product.");
    }
    var response = new ProductResponseDto
    {
        Id = productWithCategory.Id,
        Name = productWithCategory.Name,
        Price = productWithCategory.Price,
        Category = productWithCategory.Category
    };
    return Results.Ok(response);
})
.RequireAuthorization("Canteen");

// DELETE PRODUTO
app.MapDelete("/products/{id}", async (int id, AppDbContext db, HttpContext http) =>
{
    var userId = int.Parse(http.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

var school = await db.Schools.FirstOrDefaultAsync(e => e.UserId == userId);

if (school == null)
{
    return Results.NotFound("School not found.");
}

var product = await db.Products
    .FirstOrDefaultAsync(p =>
        p.Id == id &&
        p.SchoolId == school.Id);
    if (product is null)
    {
        return Results.NotFound("Product not found.");
    }
    db.Products.Remove(product);
    await db.SaveChangesAsync();
    return Results.NoContent();
})
.RequireAuthorization("Canteen");


        }
    }
}
