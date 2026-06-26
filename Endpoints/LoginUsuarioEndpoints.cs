using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using cardapio_digital.Entities;
using cardapio_digital.Dtos;
using cardapio_digital.Enums;

namespace cardapio_digital
{
public static class LoginUsuarioEndpoints
{
 public static void MapLoginUsuarioEndpoints(this WebApplication app)
{
app.MapPost("/login", async (LoginUsuarioDto dto, AppDbContext context) =>
{
    if (string.IsNullOrWhiteSpace(dto.Email))
        return Results.BadRequest("Email obrigatório");

    if (string.IsNullOrWhiteSpace(dto.Senha))
        return Results.BadRequest("Senha obrigatória");

    var usuario = await context.Usuarios
        .FirstOrDefaultAsync(x => x.Email == dto.Email);

    if (usuario == null)
        return Results.BadRequest("Usuário não encontrado");

    bool senhaCorreta = BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.SenhaHash);

    if (!senhaCorreta)
        return Results.BadRequest("Senha incorreta");

    return Results.Ok(new
    {
        usuario.Id,
        usuario.Nome,
        usuario.Email,
        usuario.Perfil
    });
});        
}     
}
}