using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cardapio_digital.Dtos;
using cardapio_digital.Entities;
using cardapio_digital.Enums;
using Microsoft.EntityFrameworkCore;

namespace cardapio_digital.Services
{
    public class UserService
    {
       private readonly AppDbContext _context;
       public UserService(AppDbContext context)
       {
        _context = context;
       }
     public async Task<IResult> Register(UserRegistrationDto dto)
    {
      if(string.IsNullOrWhiteSpace(dto.Name))
      return Results.BadRequest("Name is required");

     if(string.IsNullOrWhiteSpace(dto.Email))
     return Results.BadRequest("Email is required");

     if(string.IsNullOrWhiteSpace(dto.Password))
     return Results.BadRequest("Password is required");

     if(!Enum.IsDefined(typeof(UserRole), dto.Role))
     return Results.BadRequest("Invalid user role");

    var userExists = await _context.User.AnyAsync(x => x.Email == dto.Email);

    if(userExists)
    return Results.BadRequest("Email already registered");

    string hash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

    var user = new User
  {
    Name = dto.Name,
    Email = dto.Email,
    PasswordHash = hash,
    Role = dto.Role,
  };
  _context.User.Add(user);//Cria objeto
  await _context.SaveChangesAsync();//Salva no banco
  return Results.Ok("User created successfully"); //retorna

}
}
}