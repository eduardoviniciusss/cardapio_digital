using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using cardapio_digital.Dtos;
using cardapio_digital.Entities;
using cardapio_digital.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace cardapio_digital.Services
{
    public class LoginService
    {
      private readonly AppDbContext _context;
      private readonly IConfiguration _configuration;
      public LoginService(AppDbContext context, IConfiguration configuration)
      {
        _context = context;
        _configuration = configuration;
      }
      public async Task<IResult> Login(UserLoginDto dto)
      {
        if (string.IsNullOrWhiteSpace(dto.Email))
            return Results.BadRequest("Email is required");

        if (string.IsNullOrWhiteSpace(dto.Password))
        return Results.BadRequest("Password is required");

        var user = await _context.User.FirstOrDefaultAsync(x => x.Email == dto.Email);

        if (user == null)
        return Results.BadRequest("User not found");

        bool passwordCorrect = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

        if (!passwordCorrect)
        return Results.BadRequest("Incorrect password");

        var claims = new[]
        {
          new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
          new Claim(ClaimTypes.Email, user.Email),
          new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
        issuer: _configuration["Jwt:Issuer"],
        audience: _configuration["Jwt:Audience"],
        claims: claims,
        expires: DateTime.UtcNow.AddHours(Convert.ToDouble(_configuration["Jwt:ExpirationHours"])),
        signingCredentials: credentials
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return Results.Ok(new
        {
          Token = jwt,
          User = new
        {
          user.Id,
          user.Name,
          user.Email,
          user.Role
        }
            });
        }
    }
}