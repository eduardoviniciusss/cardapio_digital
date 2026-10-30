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

        public async Task<IResult> Register(SchoolDto dto)
        {
            if (new[] { dto.Name, dto.Address, dto.Phone }.Any(string.IsNullOrWhiteSpace))
                return Results.BadRequest("All fields are required.");

            if (dto.Shifts is null || !dto.Shifts.Any())
                return Results.BadRequest("Provide at least one shift.");

            if (string.IsNullOrWhiteSpace(dto.CanteenName))
                return Results.BadRequest("Canteen name is required.");

            if (string.IsNullOrWhiteSpace(dto.CanteenEmail))
                return Results.BadRequest("Canteen email is required.");

            if (string.IsNullOrWhiteSpace(dto.CanteenPassword) || dto.CanteenPassword.Length < 6)
                return Results.BadRequest("Canteen password must have at least 6 characters.");

            var email = dto.CanteenEmail.Trim();

            if (await _db.User.AnyAsync(u => u.Email == email))
                return Results.BadRequest("Email already registered");

            var canteenUser = new User
            {
                Name = dto.CanteenName.Trim(),
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.CanteenPassword),
                Role = UserRole.Canteen
            };

            var school = new School
            {
                Name = dto.Name,
                Address = dto.Address,
                Phone = dto.Phone,
                Shifts = dto.Shifts,
                Code = await GenerateUniqueCodeAsync(),
                User = canteenUser
            };

            _db.Schools.Add(school);
            await _db.SaveChangesAsync();

            return Results.Created($"/schools/{school.Id}", new SchoolResponseDto
            {
                Id = school.Id,
                Name = school.Name,
                Address = school.Address,
                Phone = school.Phone,
                Shifts = school.Shifts,
                Code = school.Code,
                CanteenUserId = canteenUser.Id,
                CanteenName = canteenUser.Name,
                CanteenEmail = canteenUser.Email
            });
        }

        private async Task<string> GenerateUniqueCodeAsync()
        {
            while (true)
            {
                var code = SchoolCodeGenerator.Generate();

                if (!await _db.Schools.AnyAsync(s => s.Code == code))
                    return code;
            }
        }
    }
}