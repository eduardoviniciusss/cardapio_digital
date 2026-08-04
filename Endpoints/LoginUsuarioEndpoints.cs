using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using cardapio_digital.Entities;
using cardapio_digital.Dtos;
using cardapio_digital.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace cardapio_digital
{
public static class LoginUsuarioEndpoints
{
 public static void MapLoginUsuarioEndpoints(this WebApplication app)
{
app.MapPost("/login", async (LoginUsuarioDto dto, AppDbContext context, IConfiguration configuration) =>
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

var claims = new[]
{
    new Claim(
        ClaimTypes.NameIdentifier,
        usuario.Id.ToString()),

    new Claim(
        ClaimTypes.Email,
        usuario.Email),

    new Claim(
        ClaimTypes.Role,
        usuario.Perfil.ToString())
        
};


var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]!));

var credentials = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

var token = new JwtSecurityToken(
issuer: configuration["Jwt:Issuer"],

audience: configuration["Jwt:Audience"],

claims: claims,

expires: DateTime.UtcNow.AddHours(Convert.ToDouble(
configuration["Jwt:ExpirationHours"])),

signingCredentials: credentials
);

var jwt = new JwtSecurityTokenHandler().WriteToken(token);

return Results.Ok(new
    {
      Token = jwt,

      Usuario = new
    {
        usuario.Id,
        usuario.Nome,
        usuario.Email,
        usuario.Perfil
    }
    });
});        
}     
}
}