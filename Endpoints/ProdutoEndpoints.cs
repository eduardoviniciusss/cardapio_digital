using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using cardapio_digital.Entities;

namespace cardapio_digital.Entities
{
public static class ProdutoEndpoints
{
public static void MapProdutoEndpoints(this WebApplication app)
{
//GET PRODUTO
app.MapGet("/produto",async (AppDbContext db) =>
{
    var produto = await db.Produtos.Include(p => p.Categoria).ToListAsync();
    return Results.Ok(produto);
});

//GET ID
app.MapGet("/produto/{id}", async(int id, AppDbContext db) =>
{
    var produto = await db.Produtos.FindAsync(id);
    if (produto is null)
    {
        return Results.NotFound("Produto não encontrada.");
    }
    return Results.Ok(produto);
 });

//POST PRODUTO
app.MapPost("/produto", async (AppDbContext db, Produto dto) =>
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
        CategoriaId = dto.CategoriaId
    };
    db.Produtos.Add(produto);
    await db.SaveChangesAsync();
    return Results.Created($"/produto/{produto.Id}", produto);
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
    produto.CategoriaId = dto.CategoriaId;

    await db.SaveChangesAsync();

    return Results.Ok(produto);
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
    await db.SaveChangesAsync();
    return Results.Ok(produto);
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