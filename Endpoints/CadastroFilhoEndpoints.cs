using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using cardapio_digital.Entities;
using cardapio_digital.Dtos;
using cardapio_digital.Enums;
using System.Security.Claims;

namespace cardapio_digital.Endpoints
{
    public static class CadastroFilhoEndpoints
{
public static void MapCadastroFilhoEndpoints(this WebApplication app)
{
//POST
app.MapPost("/filhos", async (CadastroFilhoDto dto,AppDbContext context, HttpContext http) =>
{ 
  if (string.IsNullOrWhiteSpace(dto.Nome))
  return Results.BadRequest("Nome obrigatório.");

  if (string.IsNullOrWhiteSpace(dto.Telefone))
  return Results.BadRequest("Telefone obrigatório.");

  if (dto.EscolaId <= 0)
    return Results.BadRequest("Escola inválida.");

if (dto.DataNascimento == default)
return Results.BadRequest("Data de nascimento obrigatória.");

var usuarioId = http.User
    .FindFirst(ClaimTypes.NameIdentifier)?
    .Value;

if (usuarioId == null)
{
    return Results.Unauthorized();
}

var pai = await context.Pais.Include(p => p.Usuario).FirstOrDefaultAsync(p => p.UsuarioId == int.Parse(usuarioId));

  if (pai == null)
    return Results.UnprocessableEntity( "Pai não encontrado");

  if (pai.Usuario?.Perfil != PerfilUsuario.Pais)
      return Results.BadRequest("Usuário informado não possui perfil Pais.");
  
  var cantina = await context.Escolas.FirstOrDefaultAsync(e => e.Id == dto.EscolaId);

  if (cantina == null)
    return Results.UnprocessableEntity("Escola não encontrada.");

  var filhoExistente = await context.Filhos.FirstOrDefaultAsync
 (f =>f.Nome == dto.Nome &&f.DataNascimento == dto.DataNascimento &&f.PaiId == pai.Id);

  if (filhoExistente != null)
 {
  if (filhoExistente.EscolaId == dto.EscolaId)
  {
  return Results.BadRequest("Este filho já está cadastrado nesta escola.");
  }
  return Results.BadRequest("Este filho já está vinculado a outra escola e não pode ser cadastrado novamente.");
 }
  var filho = new Filho
 {
    Nome = dto.Nome,
    DataNascimento = dto.DataNascimento,
    Telefone = dto.Telefone,
    PaiId = pai.Id,
    EscolaId = dto.EscolaId
};

context.Filhos.Add(filho);
await context.SaveChangesAsync();
return Results.Ok("Filho cadastrado com sucesso.");
})
.RequireAuthorization("Pais");

//GET
app.MapGet("meus-filhos", async (HttpContext http, AppDbContext db) =>
{
  var usuarioId = http.User.FindFirst(ClaimTypes.NameIdentifier)? .Value;

if (usuarioId == null)
{
    return Results.Unauthorized();
}
var pai = await db.Pais.FirstOrDefaultAsync(p => p.UsuarioId == int.Parse(usuarioId));

if (pai == null)
{
    return Results.NotFound("Pai não encontrado.");
}

 var filhos = await db.Filhos
.Where(f => f.PaiId == pai.Id)
.Include(f => f.Pais)
.Include(f => f.Escola)
.Select(f => new CadastroFilhoRespostaDto
{
  Id = f.Id,
  Nome = f.Nome,
  DataNascimento = f.DataNascimento,
  Telefone = f.Telefone,

  PaiId = f.PaiId,
  Pais = new CadastroPaisRespondeDto
  {
    Nome = f.Pais.Nome,
    Id = f.Pais.Id,
    Cpf = f.Pais.Cpf,
    Telefone = f.Pais.Telefone
  },

 EscolaId = f.EscolaId,
 Escola = new EscolaRespostaDto
 {
   Id = f.Escola.Id,
   Nome = f.Escola.Nome,
   Endereco = f.Escola.Endereco,
   Telefone = f.Escola.Telefone,
   Turnos = f.Escola.Turnos
 }
 
})
.ToListAsync();


return Results.Ok(filhos);
})
.RequireAuthorization("Pais");
  
        }  
    }
}