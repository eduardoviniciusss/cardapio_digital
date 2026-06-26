using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using cardapio_digital.Entities;
using cardapio_digital.Dtos;

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
  var response = cardapioProdutos.Select(cardapioProduto => new CardapioProdutoRespostaDto
                {
                    CardapioId = cardapioProduto.CardapioId,
                    Cardapio = cardapioProduto.Cardapio.Nome,

                    ProdutoId = cardapioProduto.ProdutoId,
                    Produto = cardapioProduto.Produto.Nome,

                    Categoria = cardapioProduto.Produto.Categoria.Nome
                });

                return Results.Ok(response);
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
    var response = new CardapioProdutoRespostaDto
    {
        CardapioId = cardapioProduto.CardapioId,
        Cardapio = cardapioProduto.Cardapio.Nome,

        ProdutoId = cardapioProduto.ProdutoId,
        Produto = cardapioProduto.Produto.Nome,

        Categoria = cardapioProduto.Produto.Categoria.Nome
    };

    return Results.Ok(response);
});

// POST CARDAPIO_PRODUTO
app.MapPost("/cardapio-produto",async (AppDbContext db, CardapioProdutoDto dto) =>
{
    var cardapioExiste = await db.Cardapios.FindAsync(dto.CardapioId);
    if (cardapioExiste is null)
    {
        return Results.BadRequest("Cardápio não encontrado.");
    }
    var produtoExiste = await db.Produtos.Include(p => p.Categoria).FirstOrDefaultAsync(p => p.Id == dto.ProdutoId);
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
    db.CardapioProdutos.Add(cardapioProduto); 
    await db.SaveChangesAsync();
     var response = new CardapioProdutoRespostaDto
    {
        CardapioId = cardapioProduto.CardapioId,
        Cardapio = cardapioExiste.Nome,

        ProdutoId = cardapioProduto.ProdutoId,
        Produto = produtoExiste.Nome,

        Categoria = produtoExiste.Categoria?.Nome ?? "Sem Categoria"
    };

    return Results.Created(
        $"/cardapio-produto/{dto.CardapioId}/{dto.ProdutoId}",
        response
    );
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
    var produtoExiste = await db.Produtos.Include(p => p.Categoria).FirstOrDefaultAsync(p => p.Id == dto.ProdutoId);
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
    var response = new CardapioProdutoRespostaDto
    {
        CardapioId = novoRegistro.CardapioId,
        Cardapio = cardapioExiste.Nome,

        ProdutoId = novoRegistro.ProdutoId,
        Produto = produtoExiste.Nome,

        Categoria = produtoExiste.Categoria?.Nome ?? "Sem Categoria"
    };

    return Results.Ok(response);
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

    var cardapioProdutoAtualizado = await db.CardapioProdutos
        .Include(cp => cp.Cardapio)
        .Include(cp => cp.Produto).ThenInclude(p => p.Categoria)
        .FirstOrDefaultAsync(cp => cp.CardapioId == cardapioProduto.CardapioId && cp.ProdutoId == cardapioProduto.ProdutoId);

    if (cardapioProdutoAtualizado is null) return Results.BadRequest("Erro ao recuperar ligação atualizada.");

    var response = new CardapioProdutoRespostaDto
    {
        CardapioId = cardapioProdutoAtualizado.CardapioId,
        Cardapio = cardapioProdutoAtualizado.Cardapio.Nome,
        ProdutoId = cardapioProdutoAtualizado.ProdutoId,
        Produto = cardapioProdutoAtualizado.Produto.Nome,
        Categoria = cardapioProdutoAtualizado.Produto.Categoria?.Nome ?? "Sem Categoria"
    };
    return Results.Ok(response);
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