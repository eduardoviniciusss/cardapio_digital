using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Xunit;
using cardapio_digital;
using cardapio_digital.Dtos;
using cardapio_digital.Entities;
using cardapio_digital.Enums;
using cardapio_digital.Services;

namespace cardapio_digital.Tests.Parents;

public class ParentServiceTests
{
    private AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task ShouldRejectRegistrationWhenUserDoesNotExist()
    {
        // Arrange
        var context = CreateContext();

        var service = new ParentService(context);

        var dto = new ParentRegistrationDto
        {
            Cpf = "12345678900",
            Phone = "81986453728",
            UserId = 999
        };

        // Act
        var result = await service.Register(dto, dto.UserId);

        // Assert
        Assert.IsType<NotFound<string>>(result);
    }

    [Fact]
    public async Task ShouldRejectRegistrationWhenCpfIsEmpty()
    {
        // Arrange
        var context = CreateContext();

        context.User.Add(new User
        {
            Name = "Eduardo",
            Email = "eduardo@gmail.com",
            PasswordHash = "123456",
            Role = UserRole.Parent
        });

        await context.SaveChangesAsync();

        var service = new ParentService(context);

        var dto = new ParentRegistrationDto
        {
            Cpf = "",
            Phone = "81986453728",
            UserId = 1
        };

        // Act
        var result = await service.Register(dto, dto.UserId);

        // Assert
        Assert.IsType<BadRequest<string>>(result);
    }

    [Fact]
    public async Task ShouldRejectRegistrationWhenPhoneIsEmpty()
    {
        // Arrange
        var context = CreateContext();

        context.User.Add(new User
        {
            Name = "Eduardo",
            Email = "eduardo@gmail.com",
            PasswordHash = "123456",
            Role = UserRole.Parent
        });

        await context.SaveChangesAsync();

        var service = new ParentService(context);

        var dto = new ParentRegistrationDto
        {
            Cpf = "12345678900",
            Phone = "",
            UserId = 1
        };

        // Act
        var result = await service.Register(dto, dto.UserId);

        // Assert
        Assert.IsType<BadRequest<string>>(result);
    }

    [Fact]
    public async Task ShouldRejectRegistrationWhenParentAlreadyExistsForUser()
    {
        // Arrange
        var context = CreateContext();

        context.User.Add(new User
        {
            Id = 1,
            Name = "Eduardo",
            Email = "eduardo@gmail.com",
            PasswordHash = "123456",
            Role = UserRole.Parent
        });

        context.Parents.Add(new Parent
        {
            Cpf = "12345678900",
            Phone = "81986453728",
            UserId = 1
        });

        await context.SaveChangesAsync();

        var service = new ParentService(context);

        var dto = new ParentRegistrationDto
        {
            Cpf = "98765432100",
            Phone = "81986453738",
            UserId = 1
        };

        // Act
        var result = await service.Register(dto, dto.UserId);

        // Assert
        Assert.IsType<BadRequest<string>>(result);
    }
}