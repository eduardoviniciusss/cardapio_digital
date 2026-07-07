using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using cardapio_digital.Entities;
using cardapio_digital.Dtos;

namespace cardapio_digital.Endpoints
{
public static class CadastroUsuarioEnpoints
{
public static void MapCadastroUsuarioEndpoints(this WebApplication app)
{
app.MapPost("/usuarios", async (CadastroUsuarioDto dto, AppDbContext context) =>
{
  if(string.IsNullOrWhiteSpace(dto.Nome))
  return Results.BadRequest("Nome obrigatório");

  if(string.IsNullOrWhiteSpace(dto.Email))
  return Results.BadRequest("Email obrigatório");

  if(string.IsNullOrWhiteSpace(dto.Senha))
  return Results.BadRequest("Senha obrigatória");

  var usuarioExiste = await context.Usuarios.AnyAsync(x => x.Email == dto.Email);

  if(usuarioExiste)
  return Results.BadRequest("Email já cadastrado");

  string hash =BCrypt.Net.BCrypt.HashPassword(dto.Senha);

  var usuario = new Usuario
  {
    Nome = dto.Nome,
    Email = dto.Email,
    SenhaHash = hash,
    Perfil = dto.Perfil,
  };
  context.Usuarios.Add(usuario);//Cria objeto
  await context.SaveChangesAsync();//Salva no banco
  return Results.Ok("Usuário criado com sucesso");//retorna 
});
        }   
    }
}