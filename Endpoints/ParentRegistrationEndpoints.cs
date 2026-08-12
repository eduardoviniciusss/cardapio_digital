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
public static class ParentRegistrationEndpoints
{
public static void  MapParentRegistrationEndpoints(this WebApplication app)
{
app.MapPost("/parents", async (ParentRegistrationDto dto, AppDbContext context, HttpContext http) =>
{
  if (string.IsNullOrWhiteSpace(dto.Cpf))
    return Results.BadRequest("CPF is required.");

  if (string.IsNullOrWhiteSpace(dto.Phone))
    return Results.BadRequest("Phone is required.");

  var userId = http.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
  if (userId == null)
    return Results.Unauthorized();

  var userIdInt = int.Parse(userId);
  var user = await context.User.FirstOrDefaultAsync(u => u.Id == userIdInt);
  if (user == null)
    return Results.NotFound("User not found.");

  if (user.Role != UserRole.Parent)
    return Results.BadRequest("The specified user does not have the Parent role.");

  var parentExists = await context.Parents.AnyAsync(p => p.UserId == userIdInt);
  if (parentExists)
    return Results.BadRequest("This user already has a parent registration.");

  var parent = new Parent
  {
    Name = dto.Name,
    Cpf = dto.Cpf,
    Phone = dto.Phone,
    UserId = userIdInt
  };
  context.Parents.Add(parent);
  await context.SaveChangesAsync();
  return Results.Created($"/parents/{parent.Id}", new
  {
    parent.Id,
    parent.Name,
    parent.Cpf,
    parent.Phone,
    parent.UserId
  });
    })
    .RequireAuthorization("Parent");      
}
        
}
}
