using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using cardapio_digital.Entities;

namespace cardapio_digital.Endpoints
{
public static class CardapioEndpoints
{
public static void MapCardapioEndpoints(this WebApplication app)
{
//GET CARDAPIO
app.MapGet("/cardapio", async (AppDbContext db) =>
{
  var cardapio = await db.Cardapios.Include(c => c.Escola).ToListAsync();
  return Results.Ok(cardapio);
});

//GET ID CARDAPIO
app.MapGet("/cardapio/{id}", async(int id, AppDbContext db) =>
{
  var cardapio = await db.Cardapios.Include(c => c.Escola)
  .FirstOrDefaultAsync(c => c.Id == id);
  if (cardapio is null)
    {
        return Results.NotFound("Cardápio não encontrado.");
    }
  return Results.Ok(cardapio);
});

//POST CARDAPIO
app.MapPost("/cardapio", async (AppDbContext db, CardapioDto dto) =>
{
    if (string.IsNullOrWhiteSpace(dto.Nome))
    {
        return Results.BadRequest("Nome é obrigatório.");
    }
   // verifica se escola existe
    var escolaExiste = await db.Escolas.FindAsync(dto.EscolaId);
    if (escolaExiste is null)
    {
        return Results.BadRequest("Escola não encontrada.");
    }
    var cardapio = new Cardapio
    {
        Nome = dto.Nome,
        EscolaId = dto.EscolaId
    };
    db.Cardapios.Add(cardapio);
    await db.SaveChangesAsync();
    return Results.Created($"/cardapio/{cardapio.Id}", cardapio);
});

//PUT CARDAPIO
app.MapPut("/cardapio/{id}", async (int id, AppDbContext db, CardapioDto dto) =>
{
    var cardapio = await db.Cardapios.FindAsync(id);
    if (cardapio is null)
    {
        return Results.NotFound();
    }
    if (string.IsNullOrWhiteSpace(dto.Nome))
    {
        return Results.BadRequest("Nome é obrigatório.");
    }
    var escolaExiste = await db.Escolas.FindAsync(dto.EscolaId);
    if (escolaExiste is null)
    {
        return Results.BadRequest("Escola não encontrada.");
    }
    cardapio.Nome = dto.Nome;
    cardapio.EscolaId = dto.EscolaId;
    await db.SaveChangesAsync();
    return Results.Ok(cardapio);
});

//PATCH CARDAPIO
app.MapPatch("/cardapio/{id}", async (int id, AppDbContext db, CardapioDto dto) =>
    {
    var cardapio = await db.Cardapios.FindAsync(id);
    if (cardapio is null)
    {
        return Results.NotFound();
    }
    if (dto.Nome is not null)
    {
        cardapio.Nome = dto.Nome;
    }
    
    if (dto.EscolaId > 0)
    {
        var escolaExiste = await db.Escolas.FindAsync(dto.EscolaId);
        if (escolaExiste is null)
        {
            return Results.BadRequest("Escola não encontrada.");
        }

        cardapio.EscolaId = dto.EscolaId;
    }
    await db.SaveChangesAsync();
    return Results.Ok(cardapio);
});

//DELETE CARDAPIO
app.MapDelete("/cardapio/{id}", async (int id, AppDbContext db) =>
{
    var cardapio = await db.Cardapios.FindAsync(id);

    if (cardapio is null)
    {
        return Results.NotFound();
    }
    db.Cardapios.Remove(cardapio);
    await db.SaveChangesAsync();
    return Results.NoContent();
}); 
        }
    }
}