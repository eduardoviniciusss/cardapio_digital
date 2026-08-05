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
public static class ProdutoEndpoints
{
public static void MapProdutoEndpoints(this WebApplication app)
{
//GET PRODUTO
app.MapGet("/products",async (AppDbContext db, HttpContext http) =>
{
    var usuarioId = int.Parse(http.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    var escola = await db.Escolas.FirstOrDefaultAsync(e => e.UsuarioId == usuarioId);

if (escola == null)
{
    return Results.NotFound("Escola não encontrada.");
}
    var produtos = await db.Produtos .Include(p => p.Categoria).Where(p => p.EscolaId == escola.Id).ToListAsync();
    var response = produtos.Select(p => new ProdutoRespostaDto
    {
        Id = p.Id,
        Nome = p.Nome,
        Preco = p.Preco,
        Categoria = p.Categoria
    }).ToList();
    return Results.Ok(response);
})
.RequireAuthorization("Cantina");

//GET ID
app.MapGet("/products/{id}", async(int id, AppDbContext db, HttpContext http) =>
{
    var usuarioId = int.Parse(http.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    var escola = await db.Escolas.FirstOrDefaultAsync(e => e.UsuarioId == usuarioId);

    if (escola == null)
    {
        return Results.NotFound("Escola não encontrada.");
    }

     var produto = await db.Produtos.Include(p => p.Categoria).FirstOrDefaultAsync(p => p.Id == id && p.EscolaId == escola.Id);
    if (produto == null)
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
 })
 .RequireAuthorization("Cantina");

//POST PRODUTO
app.MapPost("/products", async (AppDbContext db, ProdutoDto dto, HttpContext http) =>
 {
    var usuarioId = int.Parse(http.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    var escola = await db.Escolas.FirstOrDefaultAsync(e => e.UsuarioId == usuarioId);

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
    if (escola == null)
    {
    return Results.NotFound("Escola não encontrada.");
    }
    var produto = new Produto
    {
        Nome = dto.Nome,
        Preco = dto.Preco,
        CategoriaId = dto.CategoriaId,
        EscolaId = escola.Id
    
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
    return Results.Created($"/products/{produto.Id}", response);
 })
 .RequireAuthorization("Cantina");

//PUT PRODUTO
app.MapPut("/products/{id}", async (int id, AppDbContext db, ProdutoDto dto, HttpContext http) =>
 {
   var usuarioId = int.Parse(http.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

   var escola = await db.Escolas.FirstOrDefaultAsync(e => e.UsuarioId == usuarioId);

if (escola == null)
{
    return Results.NotFound("Escola não encontrada.");
}

var produto = await db.Produtos.FirstOrDefaultAsync(p =>p.Id == id && p.EscolaId == escola.Id);

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
})
.RequireAuthorization("Cantina");



// PATCH PRODUTO
app.MapPatch("/products/{id}", async (int id, AppDbContext db, ProdutoDto dto, HttpContext http) =>
{
    var usuarioId = int.Parse( http.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

var escola = await db.Escolas.FirstOrDefaultAsync(e => e.UsuarioId == usuarioId);

if (escola == null)
{
    return Results.NotFound("Escola não encontrada.");
}

var produto = await db.Produtos.FirstOrDefaultAsync(p =>p.Id == id && p.EscolaId == escola.Id);
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
})
.RequireAuthorization("Cantina");

// DELETE PRODUTO
app.MapDelete("/products/{id}", async (int id, AppDbContext db, HttpContext http) =>
{
    var usuarioId = int.Parse(http.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

var escola = await db.Escolas.FirstOrDefaultAsync(e => e.UsuarioId == usuarioId);

if (escola == null)
{
    return Results.NotFound("Escola não encontrada.");
}

var produto = await db.Produtos
    .FirstOrDefaultAsync(p =>
        p.Id == id &&
        p.EscolaId == escola.Id);
    if (produto is null)
    {
        return Results.NotFound("Produto não encontrado.");
    }
    db.Produtos.Remove(produto);
    await db.SaveChangesAsync();
    return Results.NoContent();
})
.RequireAuthorization("Cantina");


        }
    }
}