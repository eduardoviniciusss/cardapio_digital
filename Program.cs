using cardapio_digital;
using cardapio_digital.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;

var builder = WebApplication.CreateBuilder(args); 

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/healthy", () =>
{
        var result = Results.Ok(new
        {
            status = "Healthy",
            timestamp = DateTime.UtcNow
        });
        return result;
});

app.MapGet("/produto", async (AppDbContext db) =>
{
    return await db.Produtos.ToListAsync();
});

//GET com id
app.MapGet("/produto/{id}" , async (AppDbContext db,int id) => 
{
    var produto = await db.Produtos.FindAsync(id);
    return produto;
});

//POST
app.MapPost("/produto", async (AppDbContext db,ProdutoDto dto) =>
{
    var novaProduto = new Produto
    {
        Nome = dto.Nome,
        Tipo = dto.Tipo,
        Preco = dto.Preco
    };
    db.Produtos.Add(novaProduto);
    await db.SaveChangesAsync();
    return Results.Created($"/produto/{novaProduto.Id}", novaProduto);
});

//PUT
app.MapPut("/produto/{id}", async (int id,AppDbContext db,ProdutoDto dto) =>
{
    var produto = await db.Produtos.FindAsync(id);
    if (produto is null) return Results.NotFound();

    produto.Nome = dto.Nome;
    produto.Tipo = dto.Tipo;
    produto.Preco = dto.Preco;

    await db.SaveChangesAsync();
    return Results.Ok(produto);

});

app.MapPatch("/produto/{id}", async (int id, AppDbContext db, ProdutoDto dto) =>
{
    var produto = await db.Produtos.FindAsync(id);
    if (produto is null)
        return Results.NotFound();

    if (dto.Nome is not null)
        produto.Nome = dto.Nome;

    if (dto.Tipo is not null)
        produto.Tipo = dto.Tipo;

    if (dto.Preco is not null)
        produto.Preco = dto.Preco;

    await db.SaveChangesAsync();

    return Results.Ok(produto);
});

//DELETE
app.MapDelete("/produto/{id}", async (int id,AppDbContext db ) =>
{
    var produto = await db.Produtos.FindAsync(id);
    if (produto is null) return Results.NotFound("Produto não existente!");

    db.Produtos.Remove(produto);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run(); 

