using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using cardapio_digital.Dtos;
using cardapio_digital.Entities;

namespace cardapio_digital.Services
{
    public class SchoolService
    {
        private readonly AppDbContext _db;

        public SchoolService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IResult> Register(SchoolDto dto, int userId)
        {
            if (new[] { dto.Name, dto.Address, dto.Phone }
                .Any(campo => string.IsNullOrWhiteSpace(campo)))
            {
                return Results.BadRequest("All fields are required.");
            }

            if (dto.Shifts is null || !dto.Shifts.Any())
            {
                return Results.BadRequest("Provide at least one shift.");
            }

            var user = await _db.User.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return Results.NotFound("User not found.");
            }

            if (user.Role != cardapio_digital.Enums.UserRole.Canteen)
            {
                return Results.BadRequest("User must have Canteen role.");
            }

            var school = new School
            {
                Name = dto.Name!,
                Address = dto.Address!,
                Phone = dto.Phone!,
                Shifts = dto.Shifts,
                UserId = userId
            };

            _db.Schools.Add(school);
            await _db.SaveChangesAsync();

            var resposta = new SchoolResponseDto
            {
                Id = school.Id,
                Name = school.Name,
                Address = school.Address,
                Phone = school.Phone,
                Shifts = school.Shifts
            };

            return Results.Created($"/schools/{school.Id}", resposta);
        }
    }
}