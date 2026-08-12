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
public static class LoginUserEndpoints
{
 public static void MapLoginUserEndpoints(this WebApplication app)
{
app.MapPost("/login", async (UserLoginDto dto, AppDbContext context, IConfiguration configuration) =>
{
    if (string.IsNullOrWhiteSpace(dto.Email))
        return Results.BadRequest("Email is required");

    if (string.IsNullOrWhiteSpace(dto.Password))
        return Results.BadRequest("Password is required");

    var user = await context.User
        .FirstOrDefaultAsync(x => x.Email == dto.Email);

    if (user == null)
        return Results.BadRequest("User not found");

    bool passwordCorrect = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

    if (!passwordCorrect)
        return Results.BadRequest("Incorrect password");

var claims = new[]
{
    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
    new Claim(ClaimTypes.Email, user.Email),
    new Claim(ClaimTypes.Role, user.Role.ToString())
};

var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]!));
var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

var token = new JwtSecurityToken(
    issuer: configuration["Jwt:Issuer"],
    audience: configuration["Jwt:Audience"],
    claims: claims,
    expires: DateTime.UtcNow.AddHours(Convert.ToDouble(configuration["Jwt:ExpirationHours"])),
    signingCredentials: credentials
);

var jwt = new JwtSecurityTokenHandler().WriteToken(token);

return Results.Ok(new
    {
      Token = jwt,
      User = new
    {
        user.Id,
        user.Name,
        user.Email,
        user.Role
    }
    });
});        
}     
}
}
