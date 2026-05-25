using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using cardapio_digital.Entities;

namespace cardapio_digital.Endpoints
{
public static class CategoriaEndpoints
{
public static void MapCategoriaEndpoints(this WebApplication app)
{
//GET CATEGORIA
app.MapGet("/categoria", async (AppDbContext db) =>
{
   return await db.Categorias.ToListAsync();
});
//GET ID CATEGORIA
app.MapGet("/categoria/{id}", async (int id, AppDbContext db) =>
{
    var categoria = await db.Categorias.FindAsync(id);
    if (categoria is null)
    {
      return Results.NotFound("Categoria não encontarda");
    }
    return Results.Ok(categoria);
});

//POST CATEGORIA
app.MapPost("/categoria", async (AppDbContext db, CategoriaDto dto) =>
{
   if (string.IsNullOrWhiteSpace(dto.Nome))
   {
      return Results.BadRequest("Nome obrigatório.");
   }
   var categoria = new Categoria
   {
   Nome = dto.Nome!
};
db.Categorias.Add(categoria);
await db.SaveChangesAsync();
return Results.Created($"/categoria/{categoria.Id}", categoria);
});

//PUT CATEGORIA
app.MapPut("/categoria/{id}", async (int id, AppDbContext db, CategoriaDto dto) =>
{
   var categoria = await db.Categorias.FindAsync(id);
   if (categoria is null)
   {
     return Results.NotFound();
   }
   categoria.Nome = dto.Nome!;
   await db.SaveChangesAsync();
   return Results.Ok(categoria);
});

//DELETE CATEGORIA
app.MapDelete("/categoria/{id}", async (int id, AppDbContext db) =>
{
    var categoria = await db.Categorias.FindAsync(id);
    if (categoria is null)
    {
        return Results.NotFound();
    }
    db.Categorias.Remove(categoria);
    await db.SaveChangesAsync();
    return Results.NoContent();
});



        }
    }
}