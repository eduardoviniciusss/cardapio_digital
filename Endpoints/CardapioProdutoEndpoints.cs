using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using cardapio_digital.Entities;

namespace cardapio_digital.Endpoints
{
public static class CardapioProdutoEndpoints
{
public static void MapCardapioProdutoEndpoints(this WebApplication app)
{
// GET CARDAPIO_PRODUTO
app.MapGet("/cardapio-produto", async (AppDbContext db) =>
{
    var cardapioProdutos = await db.CardapioProdutos
    .Include(cp => cp.Cardapio).ThenInclude(c => c.Escola)
    .Include(cp => cp.Produto).ThenInclude(p => p.Categoria)
    .ToListAsync();
    return Results.Ok(cardapioProdutos);
});

// GET ID CARDAPIO_PRODUTO
app.MapGet("/cardapio-produto/{cardapioId}/{produtoId}",async (int cardapioId, int produtoId, AppDbContext db) =>
{
    var cardapioProduto = await db.CardapioProdutos
    .Include(cp => cp.Cardapio).ThenInclude(c => c.Escola)
    .Include(cp => cp.Produto).ThenInclude(p => p.Categoria)
    .FirstOrDefaultAsync(cp => cp.CardapioId == cardapioId && cp.ProdutoId == produtoId);
    if (cardapioProduto is null)
    {
        return Results.NotFound("Ligação não encontrada.");
    }
    return Results.Ok(cardapioProduto);
});

// POST CARDAPIO_PRODUTO
app.MapPost("/cardapio-produto",async (AppDbContext db, CardapioProdutoDto dto) =>
{
    var cardapioExiste = await db.Cardapios.FindAsync(dto.CardapioId);
    if (cardapioExiste is null)
    {
        return Results.BadRequest("Cardápio não encontrado.");
    }
    var produtoExiste = await db.Produtos.FindAsync(dto.ProdutoId);
    if (produtoExiste is null)
    {
        return Results.BadRequest("Produto não encontrado.");
    }
    var existeLigacao = await db.CardapioProdutos.AnyAsync
    (cp =>cp.CardapioId == dto.CardapioId && cp.ProdutoId == dto.ProdutoId);
    if (existeLigacao)
    {
        return Results.BadRequest("Esse produto já está no cardápio.");
    }
    var cardapioProduto = new CardapioProduto
    {
        CardapioId = dto.CardapioId,
        ProdutoId = dto.ProdutoId
    };
    db.CardapioProdutos.Add(cardapioProduto); await db.SaveChangesAsync();
    return Results.Created($"/cardapio-produto/{dto.CardapioId}/{dto.ProdutoId}",cardapioProduto);
});



// PUT CARDAPIO_PRODUTO
app.MapPut("/cardapio-produto/{cardapioId}/{produtoId}", async (int cardapioId,int produtoId,AppDbContext db,CardapioProdutoDto dto) =>
{
    var cardapioProduto = await db.CardapioProdutos.FirstOrDefaultAsync(cp =>cp.CardapioId == cardapioId && 
    cp.ProdutoId == produtoId);
    if (cardapioProduto is null)
    {
         return Results.NotFound("Ligação não encontrada.");
    }
    var cardapioExiste = await db.Cardapios.FindAsync(dto.CardapioId);
    if (cardapioExiste is null)
    {
        return Results.BadRequest("Cardápio não encontrado.");
    }
    var produtoExiste = await db.Produtos.FindAsync(dto.ProdutoId);
    if (produtoExiste is null)
    {
        return Results.BadRequest("Produto não encontrado.");
    }
    db.CardapioProdutos.Remove(cardapioProduto);
    var novoRegistro = new CardapioProduto
    {
        CardapioId = dto.CardapioId,
        ProdutoId = dto.ProdutoId
    };
    db.CardapioProdutos.Add(novoRegistro);
    await db.SaveChangesAsync();
    return Results.Ok(novoRegistro);
});



// PATCH CARDAPIO_PRODUTO
app.MapPatch("/cardapio-produto/{cardapioId}/{produtoId}", async (int cardapioId, int produtoId, AppDbContext db, CardapioProdutoDto dto) =>
{
    var cardapioProduto = await db.CardapioProdutos.FirstOrDefaultAsync
    (cp => cp.CardapioId == cardapioId && cp.ProdutoId == produtoId);
    if (cardapioProduto is null)
    {
        return Results.NotFound("Ligação não encontrada.");
    }
    if (dto.CardapioId > 0)
    {
        var cardapioExiste = await db.Cardapios.FindAsync(dto.CardapioId);
        if (cardapioExiste is null)
        {
            return Results.BadRequest("Cardápio não encontrado.");
        }
        cardapioProduto.CardapioId = dto.CardapioId;
    }
    if (dto.ProdutoId > 0)
    {
        var produtoExiste = await db.Produtos.FindAsync(dto.ProdutoId);
        if (produtoExiste is null)
        {
            return Results.BadRequest("Produto não encontrado.");
        }
        cardapioProduto.ProdutoId = dto.ProdutoId;
    }
    await db.SaveChangesAsync();
    return Results.Ok(cardapioProduto);
});



// DELETE CARDAPIO_PRODUTO
app.MapDelete("/cardapio-produto/{cardapioId}/{produtoId}",async (int cardapioId, int produtoId, AppDbContext db) =>
{
    var cardapioProduto = await db.CardapioProdutos.FirstOrDefaultAsync
    (cp => cp.CardapioId == cardapioId && cp.ProdutoId == produtoId);
    if (cardapioProduto is null)
    {
        return Results.NotFound("Ligação não encontrada.");
    }
    db.CardapioProdutos.Remove(cardapioProduto);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

        }
    }
}