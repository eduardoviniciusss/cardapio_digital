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
public static class CategoryEndpoints
{
public static void MapCategoryEndpoints(this WebApplication app)
{
//GET CATEGORIA
app.MapGet("/categories", async (AppDbContext db,HttpContext http) =>
{
    var userId = int.Parse(http.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    var school = await db.Schools.FirstOrDefaultAsync(e => e.UserId == userId);

if(school == null)
{
    return Results.NotFound("School not found.");
}
var categories = await db.Categories.Where(c => c.SchoolId == school.Id).ToListAsync();
return Results.Ok(categories);
})
.RequireAuthorization("Canteen");

//GET ID CATEGORIA
app.MapGet("/categories/{id}", async (int id, AppDbContext db, HttpContext http) =>
{
    var userId = int.Parse(http.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    var school = await db.Schools.FirstOrDefaultAsync(e => e.UserId == userId);

if(school == null)
{
    return Results.NotFound();
}
var category = await db.Categories.FirstOrDefaultAsync(c => c.Id == id && c.SchoolId == school.Id);

if (category is null)
{
    return Results.NotFound("Category not found");
}
    return Results.Ok(category);
})

.RequireAuthorization("Canteen");

//POST CATEGORIA
app.MapPost("/categories", async (AppDbContext db, CategoryDto dto, HttpContext http) =>
{

var userId = int.Parse(http.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
var school = await db.Schools
.FirstOrDefaultAsync(e => e.UserId == userId);

if(school == null)
{
    return Results.NotFound("School not found.");
}

if (string.IsNullOrWhiteSpace(dto.Name))
{
    return Results.BadRequest("Name is required.");
}
   
var category = new Category
{
    Name = dto.Name,
    SchoolId = school.Id
};

db.Categories.Add(category);
await db.SaveChangesAsync();
return Results.Created($"/categories/{category.Id}",category);
})
.RequireAuthorization("Canteen");


//PUT CATEGORIA
app.MapPut("/categories/{id}", async (int id, AppDbContext db, CategoryDto dto, HttpContext http) =>
{
   var userId = int.Parse(http.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
   var school = await db.Schools.FirstOrDefaultAsync(e => e.UserId == userId);

   if(school == null)
{
    return Results.NotFound();
}
var category = await db.Categories.FirstOrDefaultAsync(c => c.Id == id && c.SchoolId == school.Id);

   if (category is null)
   {
     return Results.NotFound();
   }
   category.Name = dto.Name!;
   await db.SaveChangesAsync();
   return Results.Ok(category);
})
.RequireAuthorization("Canteen");

//DELETE CATEGORIA
app.MapDelete("/categories/{id}", async (int id, AppDbContext db, HttpContext http) =>
{
   var userId = int.Parse(http.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
   var school = await db.Schools.FirstOrDefaultAsync(e => e.UserId == userId);
   
   if(school == null)
 {
    return Results.NotFound();
 }
    var category = await db.Categories.FirstOrDefaultAsync(c => c.Id == id && c.SchoolId == school.Id);
    if (category is null)
    {
        return Results.NotFound();
    }
    db.Categories.Remove(category);
    await db.SaveChangesAsync();
    return Results.NoContent();
})
.RequireAuthorization("Canteen");



        }
    }
}
