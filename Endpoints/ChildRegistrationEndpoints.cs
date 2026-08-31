using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using cardapio_digital.Entities;
using cardapio_digital.Dtos;
using cardapio_digital.Enums;
using System.Security.Claims;
using cardapio_digital.Services;

namespace cardapio_digital.Endpoints
{
    public static class ChildRegistrationEndpoints
{
public static void MapChildRegistrationEndpoints(this WebApplication app)
{
// POST /children
app.MapPost("/children",async (ChildRegistrationDto dto,HttpContext http,ChildService service) =>
{
    var userId = int.Parse(http.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    return await service.Register(dto, userId);
})
.RequireAuthorization("Parent");

// GET /children
app.MapGet("/children", async (HttpContext http, AppDbContext db) =>
{
  var userId = http.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

  if (userId == null)
  {
      return Results.Unauthorized();
  }

  var parent = await db.Parents.FirstOrDefaultAsync(p => p.UserId == int.Parse(userId));

  if (parent == null)
  {
      return Results.NotFound("Parent not found.");
  }

  var children = await db.Children
      .Where(f => f.ParentId == parent.Id)
      .Include(f => f.Parent)
      .Include(f => f.School)
      .Select(f => new ChildRegistrationResponseDto
      {
        Id = f.Id,
        Name = f.Name,
        BirthDate = f.BirthDate,
        Phone = f.Phone,

        ParentId = f.ParentId,
        Parent = new ParentRegistrationResponseDto
        {
          Name = f.Parent.Name,
          Id = f.Parent.Id,
          Cpf = f.Parent.Cpf,
          Phone = f.Parent.Phone
        },

        SchoolId = f.SchoolId,
        School = new SchoolResponseDto
        {
          Id = f.School.Id,
          Name = f.School.Name,
          Address = f.School.Address,
          Phone = f.School.Phone,
          Shifts = f.School.Shifts
        }
      })
      .ToListAsync();


return Results.Ok(children);
})
.RequireAuthorization("Parent");
  
        }  
    }
}
