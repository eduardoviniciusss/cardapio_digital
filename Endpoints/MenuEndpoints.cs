using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using cardapio_digital.Entities;
using cardapio_digital.Dtos;
using System.Security.Claims;

namespace cardapio_digital.Endpoints
{
public static class MenuEndpoints
{
public static void MapMenuEndpoints(this WebApplication app)
{
//GET CARDAPIO
app.MapGet("/menus", async (AppDbContext db, HttpContext http) =>
{
    var userId = int.Parse(http.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    var school = await db.Schools.FirstOrDefaultAsync(e => e.UserId == userId);

if (school == null)
{
    return Results.NotFound("School not found.");
}
  var menu = await db.Menus.Include(c => c.School).Where(c => c.SchoolId == school.Id).ToListAsync();
  var response = menu.Select(c => new MenuResponseDto { Id = c.Id, Name = c.Name, School = c.School }).ToList();
  return Results.Ok(response);
})
.RequireAuthorization("Canteen");

//GET ID CARDAPIO
app.MapGet("/menus/{id}", async(int id, AppDbContext db,  HttpContext http) =>
{
    var userId = int.Parse(http.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    var school = await db.Schools.FirstOrDefaultAsync(e => e.UserId == userId);

    if (school == null)
    {
        return Results.NotFound("School not found.");
    }

  var menu = await db.Menus.Include(c => c.School).FirstOrDefaultAsync(c => c.Id == id && c.SchoolId == school.Id);
    if (menu is null)
        {
                return Results.NotFound("Menu not found.");
        }
  var response = new MenuResponseDto { Id = menu.Id, Name = menu.Name, School = menu.School };
  return Results.Ok(response);
})
.RequireAuthorization("Canteen");

//POST CARDAPIO
app.MapPost("/menus", async (AppDbContext db, MenuDto dto, HttpContext http) =>
{
    var userId = int.Parse(http.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    var school = await db.Schools.FirstOrDefaultAsync(e => e.UserId == userId);

    if (school == null)
    {
        return Results.NotFound("School not found.");
    }

    if (string.IsNullOrWhiteSpace(dto.Name))
    {
        return Results.BadRequest("Name is required.");
    }

    var menu = new Menu
    {
        Name = dto.Name,
        SchoolId = school.Id
    };
    db.Menus.Add(menu);
    await db.SaveChangesAsync();
    var menuWithSchool = await db.Menus
        .Include(c => c.School)
        .FirstOrDefaultAsync(c => c.Id == menu.Id);
    if (menuWithSchool is null)
    {
        return Results.BadRequest("Error retrieving created menu.");
    }
    var response = new MenuResponseDto
    {
        Id = menuWithSchool.Id,
        Name = menuWithSchool.Name,
        School = menuWithSchool.School
    };
    return Results.Created($"/menus/{menu.Id}", response);
})
.RequireAuthorization("Canteen");

//PUT CARDAPIO
app.MapPut("/menus/{id}", async (int id, AppDbContext db, MenuDto dto, HttpContext http) =>
{
    var userId = int.Parse(http.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    var school = await db.Schools.FirstOrDefaultAsync(e => e.UserId == userId);

    if (school == null)
    {
    return Results.NotFound("School not found.");
    }

    var menu = await db.Menus.FirstOrDefaultAsync(p => p.Id == id && p.SchoolId == school.Id);
    if (menu is null)
    {
        return Results.NotFound();
    }
    if (string.IsNullOrWhiteSpace(dto.Name))
    {
        return Results.BadRequest("Name is required.");
    }
    menu.Name = dto.Name;

    await db.SaveChangesAsync();
    var updatedMenu = await db.Menus
        .Include(c => c.School)
        .FirstOrDefaultAsync(c => c.Id == menu.Id);
    if (updatedMenu is null)
    {
        return Results.BadRequest("Error retrieving updated menu.");
    }
    var response = new MenuResponseDto { Id = updatedMenu.Id, Name = updatedMenu.Name, School = updatedMenu.School };
    return Results.Ok(response);
})
.RequireAuthorization("Canteen");

//PATCH CARDAPIO
app.MapPatch("/menus/{id}", async (int id, AppDbContext db, MenuDto dto, HttpContext http) =>
    {
    var userId = int.Parse(http.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    var school = await db.Schools.FirstOrDefaultAsync(e => e.UserId == userId);
    if (school == null)
{
    return Results.NotFound("School not found.");
}
var menu = await db.Menus.FirstOrDefaultAsync(c => c.Id == id && c.SchoolId == school.Id);
    if (menu is null)
    {
        return Results.NotFound();
    }
    if (dto.Name is not null)
    {
        menu.Name = dto.Name;
    }
    await db.SaveChangesAsync();
    var updatedMenu = await db.Menus
        .Include(c => c.School)
        .FirstOrDefaultAsync(c => c.Id == menu.Id);
    if (updatedMenu is null)
    {
        return Results.BadRequest("Error retrieving updated menu.");
    }
    var response = new MenuResponseDto { Id = updatedMenu.Id, Name = updatedMenu.Name, School = updatedMenu.School };
    return Results.Ok(response);
})
.RequireAuthorization("Canteen");

//DELETE CARDAPIO
app.MapDelete("/menus/{id}", async (int id, AppDbContext db, HttpContext http) =>
{
   var userId = int.Parse(http.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

var school = await db.Schools.FirstOrDefaultAsync(e => e.UserId == userId);

if (school == null)
{
    return Results.NotFound("School not found.");
}

var menu = await db.Menus.FirstOrDefaultAsync(c => c.Id == id && c.SchoolId == school.Id);

    if (menu is null)
    {
        return Results.NotFound();
    }
    db.Menus.Remove(menu);
    await db.SaveChangesAsync();
    return Results.NoContent();
})
.RequireAuthorization("Canteen");
        }
    }
}
