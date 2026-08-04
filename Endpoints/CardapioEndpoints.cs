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
public static class CardapioEndpoints
{
public static void MapCardapioEndpoints(this WebApplication app)
{
//GET CARDAPIO
app.MapGet("/cardapio", async (AppDbContext db, HttpContext http) =>
{
    var usuarioId = int.Parse(http.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    var escola = await db.Escolas.FirstOrDefaultAsync(e => e.UsuarioId == usuarioId);

if (escola == null)
{
    return Results.NotFound("Escola não encontrada.");
}
  var cardapio = await db.Cardapios.Include(c => c.Escola).Where(c => c.EscolaId == escola.Id).ToListAsync();
  var response = cardapio.Select(c => new CardapioRespostaDto { Id = c.Id, Nome = c.Nome, Escola = c.Escola }).ToList();
  return Results.Ok(response);
});

//GET ID CARDAPIO
app.MapGet("/cardapio/{id}", async(int id, AppDbContext db,  HttpContext http) =>
{
    var usuarioId = int.Parse(http.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    var escola = await db.Escolas.FirstOrDefaultAsync(e => e.UsuarioId == usuarioId);

    if (escola == null)
    {
        return Results.NotFound("Escola não encontrada.");
    }

  var cardapio = await db.Cardapios.Include(c => c.Escola).FirstOrDefaultAsync(c => c.Id == id && c.EscolaId == escola.Id);
  if (cardapio is null)
    {
        return Results.NotFound("Cardápio não encontrado.");
    }
  var response = new CardapioRespostaDto { Id = cardapio.Id, Nome = cardapio.Nome, Escola = cardapio.Escola };
  return Results.Ok(response);
});

//POST CARDAPIO
app.MapPost("/cardapio", async (AppDbContext db, CardapioDto dto, HttpContext http) =>
{ 
    var usuarioId = int.Parse(http.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    var escola = await db.Escolas.FirstOrDefaultAsync(e => e.UsuarioId == usuarioId);

    if (escola == null)
    {
        return Results.NotFound("Escola não encontrada.");
    }
    
    if (string.IsNullOrWhiteSpace(dto.Nome))
    {
        return Results.BadRequest("Nome é obrigatório.");
    }

    var cardapio = new Cardapio
    {
        Nome = dto.Nome,
        EscolaId = escola.Id
    };
    db.Cardapios.Add(cardapio);
    await db.SaveChangesAsync();
    var cardapioComEscola = await db.Cardapios
        .Include(c => c.Escola)
        .FirstOrDefaultAsync(c => c.Id == cardapio.Id);
    if (cardapioComEscola is null)
    {
        return Results.BadRequest("Erro ao recuperar cardápio criado.");
    }
    var response = new CardapioRespostaDto
    {
        Id = cardapioComEscola.Id,
        Nome = cardapioComEscola.Nome,
        Escola = cardapioComEscola.Escola
    };
    return Results.Created($"/cardapio/{cardapio.Id}", response);
})
.RequireAuthorization("Cantina");

//PUT CARDAPIO
app.MapPut("/cardapio/{id}", async (int id, AppDbContext db, CardapioDto dto, HttpContext http) =>
{
    var usuarioId = int.Parse(http.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    var escola = await db.Escolas.FirstOrDefaultAsync(e => e.UsuarioId == usuarioId);

    if (escola == null)
    {
    return Results.NotFound("Escola não encontrada.");
    }   

    var cardapio = await db.Produtos.FirstOrDefaultAsync(p =>p.Id == id && p.EscolaId == escola.Id);
    if (cardapio is null)
    {
        return Results.NotFound();
    }
    if (string.IsNullOrWhiteSpace(dto.Nome))
    {
        return Results.BadRequest("Nome é obrigatório.");
    }
    cardapio.Nome = dto.Nome;

    await db.SaveChangesAsync();
    var cardapioAtualizado = await db.Cardapios
        .Include(c => c.Escola)
        .FirstOrDefaultAsync(c => c.Id == cardapio.Id);
    if (cardapioAtualizado is null)
    {
        return Results.BadRequest("Erro ao recuperar cardápio atualizado.");
    }
    var response = new CardapioRespostaDto { Id = cardapioAtualizado.Id, Nome = cardapioAtualizado.Nome, Escola = cardapioAtualizado.Escola };
    return Results.Ok(response);
})
.RequireAuthorization("Cantina");

//PATCH CARDAPIO
app.MapPatch("/cardapio/{id}", async (int id, AppDbContext db, CardapioDto dto, HttpContext http) =>
    {
    var usuarioId = int.Parse(http.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    var escola = await db.Escolas.FirstOrDefaultAsync(e => e.UsuarioId == usuarioId);
    if (escola == null)
{
    return Results.NotFound("Escola não encontrada.");
}
var cardapio = await db.Cardapios.FirstOrDefaultAsync(c => c.Id == id && c.EscolaId == escola.Id);
    if (cardapio is null)
    {
        return Results.NotFound();
    }
    if (dto.Nome is not null)
    {
        cardapio.Nome = dto.Nome;
    }
    await db.SaveChangesAsync();
    var cardapioAtualizado = await db.Cardapios
        .Include(c => c.Escola)
        .FirstOrDefaultAsync(c => c.Id == cardapio.Id);
    if (cardapioAtualizado is null)
    {
        return Results.BadRequest("Erro ao recuperar cardápio atualizado.");
    }
    var response = new CardapioRespostaDto { Id = cardapioAtualizado.Id, Nome = cardapioAtualizado.Nome, Escola = cardapioAtualizado.Escola };
    return Results.Ok(response);
})
.RequireAuthorization("Cantina");

//DELETE CARDAPIO
app.MapDelete("/cardapio/{id}", async (int id, AppDbContext db, HttpContext http) =>
{
   var usuarioId = int.Parse(http.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

var escola = await db.Escolas.FirstOrDefaultAsync(e => e.UsuarioId == usuarioId);

if (escola == null)
{
    return Results.NotFound("Escola não encontrada.");
}

var cardapio = await db.Cardapios.FirstOrDefaultAsync(c =>c.Id == id && c.EscolaId == escola.Id);

    if (cardapio is null)
    {
        return Results.NotFound();
    }
    db.Cardapios.Remove(cardapio);
    await db.SaveChangesAsync();
    return Results.NoContent();
})
.RequireAuthorization("Cantina");
        }
    }
}