using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using cardapio_digital.Entities;
using cardapio_digital.Dtos;
using cardapio_digital.Enums;
using cardapio_digital.Services;


namespace cardapio_digital.Endpoints
{
public static class UserRegistrationEndpoints
{
public static void MapUserRegistrationEndpoints(this WebApplication app)
{
app.MapPost("/users",
async (UserRegistrationDto dto,UserService service) =>
{
    return await service.Register(dto);
});
        }
    }
}
