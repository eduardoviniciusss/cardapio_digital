using System.Runtime.CompilerServices;
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
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
          .UseSnakeCaseNamingConvention ());

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

//GET CARDAPIO
app.MapGet("/cardapio", async (AppDbContext db) =>
{
  var cardapio = await db.Cardapios.Include(c => c.Escola).ToListAsync();
  return Results.Ok(cardapio);
});

//GET ID CARDAPIO
app.MapGet("cardapio/{id}", async(int id, AppDbContext db) =>
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

app.Run(); 

