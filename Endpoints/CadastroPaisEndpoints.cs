using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using cardapio_digital.Entities;
using cardapio_digital.Dtos;
using cardapio_digital.Enums;


namespace cardapio_digital.Endpoints
{
public static class CadastroPaisEndpoints
{
public static void  MapCadastroPaisEndpoints(this WebApplication app)
{
app.MapPost("/pais", async (CadastroPaisDto dto, AppDbContext context) =>
{
  if (string.IsNullOrWhiteSpace(dto.Cpf))
  return Results.BadRequest("CPF obrigatório.");

  if (string.IsNullOrWhiteSpace(dto.Telefone))
  return Results.BadRequest("Telefone obrigatório.");

  if (dto.UsuarioId <= 0)
  return Results.BadRequest("Usuário inválido.");

  var usuario = await context.Usuarios.FirstOrDefaultAsync(u => u.Id == dto.UsuarioId);

  if (usuario == null)
  return Results.NotFound("Usuário não encontrado.");

  if (usuario.Perfil != PerfilUsuario.Pais)
  return Results.BadRequest("O usuário informado não possui perfil de Pai.");

  var paiExiste = await context.Pais.AnyAsync(p => p.UsuarioId == dto.UsuarioId);

  if (paiExiste)
  return Results.BadRequest("Este usuário já possui um cadastro de pai.");

  var pai = new Pais
{
   Nome = dto.Nome,
   Cpf = dto.Cpf,
   Telefone = dto.Telefone,
   UsuarioId = dto.UsuarioId
};
context.Pais.Add(pai);
await context.SaveChangesAsync();
return Results.Created($"/pais/{pai.Id}", new
{
  pai.Id,
  pai.Nome,
  pai.Cpf,
  pai.Telefone,
  pai.UsuarioId
});
    });        
}
        
}
}