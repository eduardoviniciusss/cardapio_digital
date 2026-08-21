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
    public class ParentService
    {
        private readonly AppDbContext _context;

        public ParentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IResult> Register(ParentRegistrationDto dto, int userId)
        {
            if (string.IsNullOrWhiteSpace(dto.Cpf))
                return Results.BadRequest("CPF is required.");

            if (string.IsNullOrWhiteSpace(dto.Phone))
                return Results.BadRequest("Phone is required.");

            var user = await _context.User.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return Results.NotFound("User not found.");

            if (user.Role != UserRole.Parent)
                return Results.BadRequest("The specified user does not have the Parent role.");

            var parentExists = await _context.Parents.AnyAsync(p => p.UserId == userId);
            if (parentExists)
                return Results.BadRequest("This user already has a parent registration.");

            var parent = new Parent
            {
                Name = dto.Name,
                Cpf = dto.Cpf,
                Phone = dto.Phone,
                UserId = userId
            };

            _context.Parents.Add(parent);
            await _context.SaveChangesAsync();

            return Results.Created($"/parents/{parent.Id}", new
            {
                parent.Id,
                parent.Name,
                parent.Cpf,
                parent.Phone,
                parent.UserId
            });
        }
    }
}