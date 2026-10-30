using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using cardapio_digital.Entities;
using cardapio_digital.Dtos;
using cardapio_digital.Enums;
using System.Security.Claims;
using cardapio_digital.Services;


namespace cardapio_digital.Endpoints
{
public static class SchoolEndpoints
{
public static void MapSchoolEndpoints(this WebApplication app)
{
 //GET ESCOLA - a escola do usuário logado
app.MapGet("/schools", async (AppDbContext db,HttpContext http) =>
{
    var userId = int.Parse(http.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    var school = await db.Schools.FirstOrDefaultAsync(e =>e.UserId == userId);
    if(school == null)
{
    return Results.NotFound();
}
return Results.Ok(school);
    
}).RequireAuthorization("Canteen");

//GET TODAS AS ESCOLAS - só Administrator
app.MapGet("/schools/all", async (AppDbContext db) =>
{
    var schools = await db.Schools
        .Include(s => s.User)
        .Select(s => new SchoolResponseDto
        {
            Id = s.Id,
            Name = s.Name,
            Address = s.Address,
            Phone = s.Phone,
            Shifts = s.Shifts,
            Code = s.Code,
            CanteenUserId = s.UserId,
            CanteenName = s.User.Name,
            CanteenEmail = s.User.Email
            
        })
        .ToListAsync();

    return Results.Ok(schools);
})
.RequireAuthorization("Administrator");

//GET ID ESCOLA
app.MapGet("/schools/{id}", async (int id, AppDbContext db, HttpContext http) =>
{
  var userId = int.Parse(http.User.FindFirst(ClaimTypes.NameIdentifier)!.Value );

 var school = await db.Schools.FirstOrDefaultAsync(e =>e.Id == id &&e.UserId == userId);

 if (school is null)
{
    return Results.NotFound("School not found.");
}
return Results.Ok(school);
})
.RequireAuthorization("Canteen");

//POST ESCOLA
app.MapPost("/schools",async (SchoolDto dto, SchoolService service) => 
await service.Register(dto))
.RequireAuthorization("Administrator");


//PUT ESCOLA
app.MapPut("/schools/{id}",  async (int id, AppDbContext db, SchoolDto dto) =>
{
    
    var school = await db.Schools.FirstOrDefaultAsync(e => e.Id == id);
    if(school is null)
    {
        return Results.NotFound("School not found.");
    }
    school.Name = dto.Name;
    school.Address = dto.Address;
    school.Phone = dto.Phone;
    school.Shifts = dto.Shifts;
    await db.SaveChangesAsync();
    return Results.Ok(school);
})
.RequireAuthorization("Administrator");

//PATCH ESCOLA
app.MapPatch("/schools/{id}", async (int id, AppDbContext db, SchoolDto dto) =>
{
    var school = await db.Schools.FirstOrDefaultAsync(e => e.Id == id);
    if(school is null)
    {
        return Results.NotFound("School not found.");
    }
    if (school is null)
        return Results.NotFound();
    if (dto.Name is not null)
        school.Name = dto.Name;
    if (dto.Address is not null)
        school.Address = dto.Address;
    if (dto.Phone is not null)
        school.Phone = dto.Phone;
    if (dto.Shifts is not null)
        school.Shifts = dto.Shifts;
    await db.SaveChangesAsync();
    return Results.Ok(school);
})
.RequireAuthorization("Administrator");

//DELETE 
app.MapDelete("/schools/{id}", async (int id, AppDbContext db) =>
{
    var school = await db.Schools
        .Include(s => s.User)
        .FirstOrDefaultAsync(s => s.Id == id);

    if (school is null)
        return Results.NotFound("School not found!");

    db.Schools.Remove(school);

    if (school.User is not null)
        db.User.Remove(school.User);

    await db.SaveChangesAsync();

    return Results.NoContent();
})
.RequireAuthorization("Administrator");

        }
    }
}
