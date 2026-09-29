using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using cardapio_digital.Dtos;
using cardapio_digital.Entities;
using cardapio_digital.Enums;

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

             if (dto.CanteenUserId <= 0)
            {
            return Results.BadRequest("Provide the Canteen user who owns this school (CanteenUserId).");
            }
            var canteenUser = await _db.User.FirstOrDefaultAsync(u => u.Id == dto.CanteenUserId);

            if (canteenUser == null)
            {
            return Results.BadRequest("The Cantina user provided does not exist.");
            }

            if (canteenUser.Role != UserRole.Canteen)
            {
            return Results.BadRequest("The user provided doesn't have the Canteen role.");
            }  

           var alreadyHasSchool = await _db.Schools.AnyAsync(s => s.UserId == dto.CanteenUserId);

           if (alreadyHasSchool)
            {
            return Results.BadRequest("This Cantina user already has a school linked.");
            }
            

            var school = new School
            {
                Name = dto.Name!,
                Address = dto.Address!,
                Phone = dto.Phone!,
                Shifts = dto.Shifts,
                UserId = dto.CanteenUserId
            };

            _db.Schools.Add(school);
            await _db.SaveChangesAsync();

            var resposta = new SchoolResponseDto
            {
                Id = school.Id,
                Name = school.Name,
                Address = school.Address,
                Phone = school.Phone,
                Shifts = school.Shifts,
                CanteenUserId = canteenUser.Id,
                CanteenName = canteenUser.Name

            };

            return Results.Created($"/schools/{school.Id}", resposta);
        }
    }
}