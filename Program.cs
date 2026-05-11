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

//GET
app.MapGet("/escola", async (AppDbContext db) =>
{
   return await db.Escolas.ToListAsync(); 
});

//GET ID
app.MapGet("/escola/{id}", async (int id, AppDbContext db) =>
{
    var escola = await db.Escolas.FindAsync(id);

    return escola;
});

//POST
app.MapPost("/escola",async(AppDbContext db,EscolaDto dto) =>
{
    var escola = new Escola
    {
       Nome = dto.Nome,
       Endereco = dto.Endereco,
       Telefone = dto.Telefone,
       Turno = dto.Turno 
    };

    db.Escolas.Add(escola);
    await db.SaveChangesAsync();
    return Results.Created($"/escola/{escola.Id}",escola);
});

//PUT
app.MapPut("/escola{id}", async (int id, AppDbContext db, EscolaDto dto) => 
{
   var escola = await db.Escolas.FindAsync(id);
   if (escola is null) return Results.NotFound();
   
   escola.Nome = dto.Nome;
   escola.Endereco = dto.Endereco;
   escola.Telefone = dto.Telefone;
   escola.Turno = dto.Turno;

   await db.SaveChangesAsync();
   return Results.Ok(escola); 
});

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

app.MapDelete("/escola/{id}", async (int id,AppDbContext db ) =>
{
    var escola = await db.Escolas.FindAsync(id);
    if (escola is null) return Results.NotFound("Escola não existente!");

    db.Escolas.Remove(escola);
    await db.SaveChangesAsync();
    return Results.NoContent();
});


app.Run(); 

