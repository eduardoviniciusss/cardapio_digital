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
public static class CategoriaEndpoints
{
public static void MapCategoriaEndpoints(this WebApplication app)
{
//GET CATEGORIA
app.MapGet("/categories", async (AppDbContext db,HttpContext http) =>
{
    var usuarioId = int.Parse(http.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    var escola = await db.Escolas.FirstOrDefaultAsync(e => e.UsuarioId == usuarioId);

if(escola == null)
{
    return Results.NotFound("Escola não encontrada.");
}
var categorias = await db.Categorias.Where(c => c.EscolaId == escola.Id).ToListAsync();
return Results.Ok(categorias);
})
.RequireAuthorization("Cantina");

//GET ID CATEGORIA
app.MapGet("/categories/{id}", async (int id, AppDbContext db, HttpContext http) =>
{
    var usuarioId = int.Parse(http.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    var escola = await db.Escolas.FirstOrDefaultAsync(e => e.UsuarioId == usuarioId);

if(escola == null)
{
    return Results.NotFound();
}
var categoria = await db.Categorias.FirstOrDefaultAsync(c =>c.Id == id && c.EscolaId == escola.Id);

if (categoria is null)
{
    return Results.NotFound("Categoria não encontarda");
}
    return Results.Ok(categoria);
})

.RequireAuthorization("Cantina");

//POST CATEGORIA
app.MapPost("/categories", async (AppDbContext db, CategoriaDto dto, HttpContext http) =>
{

var usuarioId = int.Parse(http.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
var escola = await db.Escolas
.FirstOrDefaultAsync(e => e.UsuarioId == usuarioId);

if(escola == null)
{
    return Results.NotFound(
        "Escola não encontrada."
    );
}

if (string.IsNullOrWhiteSpace(dto.Nome))
{
    return Results.BadRequest("Nome obrigatório.");
}
   
var categoria = new Categoria
{
    Nome = dto.Nome,
    EscolaId = escola.Id
};

db.Categorias.Add(categoria);
await db.SaveChangesAsync();
return Results.Created(
$"/categories/{categoria.Id}",categoria);
})
.RequireAuthorization("Cantina");


//PUT CATEGORIA
app.MapPut("/categories/{id}", async (int id, AppDbContext db, CategoriaDto dto, HttpContext http) =>
{
   var usuarioId = int.Parse(http.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
   var escola = await db.Escolas.FirstOrDefaultAsync(e => e.UsuarioId == usuarioId);

   if(escola == null)
{
    return Results.NotFound();
}
var categoria = await db.Categorias.FirstOrDefaultAsync(c =>c.Id == id && c.EscolaId == escola.Id);

   if (categoria is null)
   {
     return Results.NotFound();
   }
   categoria.Nome = dto.Nome!;
   await db.SaveChangesAsync();
   return Results.Ok(categoria);
})
.RequireAuthorization("Cantina");

//DELETE CATEGORIA
app.MapDelete("/categories/{id}", async (int id, AppDbContext db, HttpContext http) =>
{
   var usuarioId = int.Parse(http.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
   var escola = await db.Escolas.FirstOrDefaultAsync(e => e.UsuarioId == usuarioId);
   
   if(escola == null)
 {
    return Results.NotFound();
 }
   var categoria = await db.Categorias.FirstOrDefaultAsync(c =>c.Id == id && c.EscolaId == escola.Id);
    if (categoria is null)
    {
        return Results.NotFound();
    }
    db.Categorias.Remove(categoria);
    await db.SaveChangesAsync();
    return Results.NoContent();
})
.RequireAuthorization("Cantina");



        }
    }
}