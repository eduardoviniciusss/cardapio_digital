using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using cardapio_digital.Entities;

namespace cardapio_digital.Endpoints
{
public static class EscolaEndpoints
{
public static void MapCardapioEndpoints(this WebApplication app)
{
 //GET ESCOLA
app.MapGet("/escola", async (AppDbContext db) =>
{
   return await db.Escolas.ToListAsync(); 
});

//GET ID ESCOLA
app.MapGet("/escola/{id}", async (int id, AppDbContext db) =>
{
    var escola = await db.Escolas.FindAsync(id);
    if (escola is null)
    {
        return Results.NotFound("Escola não encontrada.");
    }
   return Results.Ok(escola);
});

//POST ESCOLA
app.MapPost("/escola", async (AppDbContext db, EscolaDto dto) =>
{
    if (new[] { dto.Nome, dto.Endereco, dto.Telefone, dto.Turno }
        .Any(campo => campo is null))
    {
        return Results.BadRequest("Todos os campos são obrigatórios.");
    }
    var escola = new Escola
    {
        Nome = dto.Nome!,
        Endereco = dto.Endereco!,
        Telefone = dto.Telefone!,
        Turno = dto.Turno!
    };
    db.Escolas.Add(escola);
    await db.SaveChangesAsync();
    return Results.Created($"/escola/{escola.Id}", escola);
});

//PUT ESCOLA
app.MapPut("/escola/{id}", async (int id, AppDbContext db, EscolaDto dto) =>
{
    if (await db.Escolas.FindAsync(id) is not Escola escola)
        return Results.NotFound();
    escola.Nome = dto.Nome ?? escola.Nome;
    escola.Endereco = dto.Endereco ?? escola.Endereco;
    escola.Telefone = dto.Telefone ?? escola.Telefone;
    escola.Turno = dto.Turno ?? escola.Turno;
    await db.SaveChangesAsync();
    return Results.Ok(escola);
});

//PATCH ESCOLA
app.MapPatch("/escola/{id}", async (int id, AppDbContext db, EscolaDto dto) =>
{
    var escola = await db.Escolas.FindAsync(id);
    if (escola is null)
        return Results.NotFound();
    if (dto.Nome is not null)
        escola.Nome = dto.Nome;
    if (dto.Endereco is not null)
        escola.Endereco = dto.Endereco;
    if (dto.Telefone is not null)
        escola.Telefone = dto.Telefone;
    if (dto.Turno is not null)
        escola.Turno = dto.Turno;
    await db.SaveChangesAsync();
    return Results.Ok(escola);
});

//DELETE 
app.MapDelete("/escola/{id}", async (int id,AppDbContext db ) =>
{
    var escola = await db.Escolas.FindAsync(id);
    if (escola is null) return Results.NotFound("Escola não existente!");
    db.Escolas.Remove(escola);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

        }
    }
}