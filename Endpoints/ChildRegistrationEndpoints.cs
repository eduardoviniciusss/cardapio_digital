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
    public static class ChildRegistrationEndpoints
{
public static void MapChildRegistrationEndpoints(this WebApplication app)
{
// POST /children
app.MapPost("/children", async (ChildRegistrationDto dto, AppDbContext context, HttpContext http) =>
{ 
  if (string.IsNullOrWhiteSpace(dto.Name))
    return Results.BadRequest("Name is required.");

  if (string.IsNullOrWhiteSpace(dto.Phone))
    return Results.BadRequest("Phone is required.");

  if (dto.SchoolId <= 0)
    return Results.BadRequest("Invalid school.");

  if (dto.BirthDate == default)
    return Results.BadRequest("Birth date is required.");

  var userId = http.User
      .FindFirst(ClaimTypes.NameIdentifier)?
      .Value;

  if (userId == null)
  {
      return Results.Unauthorized();
  }

  var parent = await context.Parents.Include(p => p.User).FirstOrDefaultAsync(p => p.UserId == int.Parse(userId));

  if (parent == null)
    return Results.UnprocessableEntity("Parent not found.");

  if (parent.User?.Role != UserRole.Parent)
    return Results.BadRequest("The authenticated user is not a parent.");
  
  var school = await context.Schools.FirstOrDefaultAsync(e => e.Id == dto.SchoolId);

  if (school == null)
    return Results.UnprocessableEntity("School not found.");

  var existingChild = await context.Children.FirstOrDefaultAsync(
    f => f.Name == dto.Name && f.BirthDate == dto.BirthDate && f.ParentId == parent.Id);

  if (existingChild != null)
  {
    if (existingChild.SchoolId == dto.SchoolId)
    {
      return Results.BadRequest("This child is already registered at this school.");
    }
    return Results.BadRequest("This child is already linked to another school and cannot be registered again.");
  }

  var child = new Child
  {
    Name = dto.Name,
    BirthDate = dto.BirthDate,
    Phone = dto.Phone,
    ParentId = parent.Id,
    SchoolId = dto.SchoolId
  };

context.Children.Add(child);
await context.SaveChangesAsync();
return Results.Ok("Child registered successfully.");
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
