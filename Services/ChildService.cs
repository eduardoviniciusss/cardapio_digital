using Microsoft.EntityFrameworkCore;
using cardapio_digital.Dtos;
using cardapio_digital.Entities;
using cardapio_digital.Enums;

namespace cardapio_digital.Services;

public class ChildService
{
    private readonly AppDbContext _context;

    public ChildService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IResult> Register(ChildRegistrationDto dto, int userId)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            return Results.BadRequest("Name is required.");

        if (string.IsNullOrWhiteSpace(dto.Phone))
            return Results.BadRequest("Phone is required.");

        if (string.IsNullOrWhiteSpace(dto.SchoolCode))
            return Results.BadRequest("School code is required.");

        if (dto.BirthDate == default)
            return Results.BadRequest("Birth date is required.");

        var parent = await _context.Parents
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.UserId == userId);

        if (parent == null)
            return Results.UnprocessableEntity("Parent not found.");

        if (parent.User?.Role != UserRole.Parent)
            return Results.BadRequest("The authenticated user is not a parent.");

        var code = dto.SchoolCode.Trim().ToUpperInvariant();

        var school = await _context.Schools
            .FirstOrDefaultAsync(e => e.Code == code);

        if (school == null)
            return Results.UnprocessableEntity("Invalid school code.");

        var existingChild = await _context.Children
            .FirstOrDefaultAsync(f =>
                f.Name == dto.Name &&
                f.BirthDate == dto.BirthDate &&
                f.ParentId == parent.Id);

        if (existingChild != null)
        {
            if (existingChild.SchoolId == school.Id)
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
            SchoolId = school.Id
        };

        _context.Children.Add(child);

        await _context.SaveChangesAsync();

        return Results.Ok("Child registered successfully.");
    }
}