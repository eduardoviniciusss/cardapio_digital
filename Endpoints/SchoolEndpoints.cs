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
public static class SchoolEndpoints
{
public static void MapSchoolEndpoints(this WebApplication app)
{
 //GET ESCOLA
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
app.MapPost("/schools",
async (SchoolDto dto,HttpContext http,SchoolService service)=>
{
    var userId = int.Parse(http.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    return await service.Register(dto, userId);
})
.RequireAuthorization("Administrator");


//PUT ESCOLA
app.MapPut("/schools/{id}",  async (int id, AppDbContext db, SchoolDto dto, HttpContext http) =>
{
    var userId = int.Parse(http.User.FindFirst(ClaimTypes.NameIdentifier)!.Value );
    var school = await db.Schools.FirstOrDefaultAsync(e =>e.Id == id && e.UserId == userId );
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
.RequireAuthorization("Canteen");

//PATCH ESCOLA
app.MapPatch("/schools/{id}", async (int id, AppDbContext db, SchoolDto dto, HttpContext http) =>
{
    var userId = int.Parse(http.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    var school = await db.Schools.FirstOrDefaultAsync(e =>e.Id == id && e.UserId == userId);
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
.RequireAuthorization("Canteen");

//DELETE 
app.MapDelete("/schools/{id}", async (int id,AppDbContext db ) =>
{
    var school = await db.Schools.FindAsync(id);
    if (school is null) return Results.NotFound("School not found!");
    db.Schools.Remove(school);
    await db.SaveChangesAsync();
    return Results.NoContent();
})
.RequireAuthorization("Administrator");

        }
    }
}
