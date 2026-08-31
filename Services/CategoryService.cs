using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using cardapio_digital.Dtos;
using cardapio_digital.Entities;

namespace cardapio_digital.Services
{
    public class CategoryService
    {
        private readonly AppDbContext _db;

        public CategoryService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IResult> Register(CategoryDto dto, int userId)
        {
        var school = await _db.Schools.FirstOrDefaultAsync(e => e.UserId == userId);
        if (school == null)
        {
            return Results.NotFound("School not found.");
        }

        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            return Results.BadRequest("Name is required.");
        }

        var category = new Category
        {
            Name = dto.Name,
            SchoolId = school.Id
        };

        _db.Categories.Add(category);
        await _db.SaveChangesAsync();
        return Results.Created($"/categories/{category.Id}", category);
    }

        

        
    }
}
