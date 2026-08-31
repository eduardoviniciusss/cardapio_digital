using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using cardapio_digital.Entities;
using cardapio_digital.Dtos;
using cardapio_digital.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using cardapio_digital.Services;

namespace cardapio_digital
{
public static class LoginUserEndpoints
{
 public static void MapLoginUserEndpoints(this WebApplication app)
{
app.MapPost("/login", async (UserLoginDto dto,LoginService service) =>
{
    return await service.Login(dto);
});        
    }     
  }
}