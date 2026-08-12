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
public static class UserRegistrationEndpoints
{
public static void MapUserRegistrationEndpoints(this WebApplication app)
{
app.MapPost("/users", async (UserRegistrationDto dto, AppDbContext context) =>
{
  if(string.IsNullOrWhiteSpace(dto.Name))
    return Results.BadRequest("Name is required");

  if(string.IsNullOrWhiteSpace(dto.Email))
    return Results.BadRequest("Email is required");

  if(string.IsNullOrWhiteSpace(dto.Password))
    return Results.BadRequest("Password is required");

  var userExists = await context.User.AnyAsync(x => x.Email == dto.Email);

  if(userExists)
    return Results.BadRequest("Email already registered");

  string hash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

  var user = new User
  {
    Name = dto.Name,
    Email = dto.Email,
    PasswordHash = hash,
    Role = dto.Role,
  };
 context.User.Add(user);//Cria objeto
  await context.SaveChangesAsync();//Salva no banco
  return Results.Ok("User created successfully"); //retorna 

});
        }
    }
}
