using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using cardapio_digital.Entities;
using cardapio_digital.Dtos;

namespace cardapio_digital.Endpoints
{
public static class ProdutoEndpoints
{
public static void MapProdutoEndpoints(this WebApplication app)
{
//GET PRODUTO
app.MapGet("/produto",async (AppDbContext db) =>
{
    var produtos = await db.Produtos.Include(p => p.Categoria).ToListAsync();
    var response = produtos.Select(p => new ProdutoRespostaDto
    {
        Id = p.Id,
        Nome = p.Nome,
        Preco = p.Preco,
        Categoria = p.Categoria
    }).ToList();
    return Results.Ok(response);
});

//GET ID
app.MapGet("/produto/{id}", async(int id, AppDbContext db) =>
{
    var produto = await db.Produtos
        .Include(p => p.Categoria)
        .FirstOrDefaultAsync(p => p.Id == id);
    if (produto is null)
    {
        return Results.NotFound("Produto não encontrado.");
    }
    var response = new ProdutoRespostaDto
    {
        Id = produto.Id,
        Nome = produto.Nome,
        Preco = produto.Preco,
        Categoria = produto.Categoria
    };
    return Results.Ok(response);
 });

//POST PRODUTO
app.MapPost("/produto", async (AppDbContext db, ProdutoDto dto) =>
 {
     if (string.IsNullOrWhiteSpace(dto.Nome))
    {
        return Results.BadRequest("Nome obrigatório.");
    }
    //verifica se a categoria existe
    var categoriaExiste = await db.Categorias.FindAsync(dto.CategoriaId);
    if (categoriaExiste is null)
    {
        return Results.BadRequest("Categoria não encontrada.");
    }
    var produto = new Produto
    {
        Nome = dto.Nome,
        Preco = dto.Preco,
        CategoriaId = dto.CategoriaId
    };
    db.Produtos.Add(produto);
    await db.SaveChangesAsync();
    var produtoComCategoria = await db.Produtos
        .Include(p => p.Categoria)
        .FirstOrDefaultAsync(p => p.Id == produto.Id);
    if (produtoComCategoria is null)
    {
        return Results.BadRequest("Erro ao recuperar produto criado.");
    }
    var response = new ProdutoRespostaDto
    {
        Id = produtoComCategoria.Id,
        Nome = produtoComCategoria.Nome,
        Preco = produtoComCategoria.Preco,
        Categoria = produtoComCategoria.Categoria
    };
    return Results.Created($"/produto/{produto.Id}", response);
 });

//PUT PRODUTO
app.MapPut("/produto/{id}", async (int id, AppDbContext db, ProdutoDto dto) =>
 {
    var produto = await db.Produtos.FindAsync(id);
    if (produto is null)
    {
        return Results.NotFound("Produto não encontrado.");
    }

    if (string.IsNullOrWhiteSpace(dto.Nome))
    {
        return Results.BadRequest("Nome obrigatório.");
    }

    var categoriaExiste = await db.Categorias.FindAsync(dto.CategoriaId);

    if (categoriaExiste is null)
    {
        return Results.BadRequest("Categoria não encontrada.");
    }

    produto.Nome = dto.Nome;
    produto.Preco = dto.Preco;
    produto.CategoriaId = dto.CategoriaId;

    await db.SaveChangesAsync();

    var produtoComCategoria = await db.Produtos
        .Include(p => p.Categoria)
        .FirstOrDefaultAsync(p => p.Id == produto.Id);
    if (produtoComCategoria is null)
    {
        return Results.BadRequest("Erro ao recuperar produto atualizado.");
    }
    var response = new ProdutoRespostaDto
    {
        Id = produtoComCategoria.Id,
        Nome = produtoComCategoria.Nome,
        Preco = produtoComCategoria.Preco,
        Categoria = produtoComCategoria.Categoria
    };
    return Results.Ok(response);
});



// PATCH PRODUTO
app.MapPatch("/produto/{id}", async (int id, AppDbContext db, ProdutoDto dto) =>
{
    var produto = await db.Produtos.FindAsync(id);
    if (produto is null)
    {
      return Results.NotFound("Produto não encontrado.");
    }
    if (!string.IsNullOrWhiteSpace(dto.Nome))
    {
       produto.Nome = dto.Nome;
    }
    if (dto.CategoriaId > 0)
    {
        var categoriaExiste = await db.Categorias.FindAsync(dto.CategoriaId);
        if (categoriaExiste is null)
        {
           return Results.BadRequest("Categoria não encontrada.");
        }
        produto.CategoriaId = dto.CategoriaId;
    }
    if (dto.Preco > 0)
    {
        produto.Preco = dto.Preco;
    }
    await db.SaveChangesAsync();
    var produtoComCategoria = await db.Produtos
        .Include(p => p.Categoria)
        .FirstOrDefaultAsync(p => p.Id == produto.Id);
    if (produtoComCategoria is null)
    {
        return Results.BadRequest("Erro ao recuperar produto atualizado.");
    }
    var response = new ProdutoRespostaDto
    {
        Id = produtoComCategoria.Id,
        Nome = produtoComCategoria.Nome,
        Preco = produtoComCategoria.Preco,
        Categoria = produtoComCategoria.Categoria
    };
    return Results.Ok(response);
});

// DELETE PRODUTO
app.MapDelete("/produto/{id}", async (int id, AppDbContext db) =>
{
    var produto = await db.Produtos.FindAsync(id);
    if (produto is null)
    {
        return Results.NotFound("Produto não encontrado.");
    }
    db.Produtos.Remove(produto);
    await db.SaveChangesAsync();
    return Results.NoContent();
});


        }
    }
}