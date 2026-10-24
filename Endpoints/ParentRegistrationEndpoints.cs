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
public static class ParentRegistrationEndpoints
{
public static void  MapParentRegistrationEndpoints(this WebApplication app)
{
app.MapPost("/parents",
async (ParentRegistrationDto dto,HttpContext http,ParentService service) =>
{
    var userId = http.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    if (userId == null)
        return Results.Unauthorized();

    return await service.Register(dto, int.Parse(userId));
})
  .RequireAuthorization("Parent");

app.MapGet("/parents/users", async (AppDbContext db) =>
{
    var parents = await db.User
        .Where(u => u.Role == UserRole.Parent)
        .OrderBy(u => u.Name)
        .Select(u => new ParentUserResponseDto
        {
            UserId = u.Id,
            Name = u.Name,
            Email = u.Email,
            HasRegistration = db.Parents.Any(p => p.UserId == u.Id)
        })
        .ToListAsync();

    return Results.Ok(parents);
})
.RequireAuthorization("Administrator");
}

}
}
