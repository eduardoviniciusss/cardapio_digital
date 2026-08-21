using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Xunit;
using cardapio_digital;
using cardapio_digital.Dtos;
using cardapio_digital.Entities;
using cardapio_digital.Enums;
using cardapio_digital.Services;

namespace cardapio_digital.Tests.Schools;

public class SchoolServiceTests
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

        var service = new SchoolService(context);

        var dto = new SchoolDto
        {
            Name = "ABC",
            Address = "Rua A",
            Phone = "81986453728",
            Shifts = new List<Shift>
            {
                Shift.Morning
            }
        };

        // Act
        var result = await service.Register(dto, 999);

        // Assert
        Assert.IsType<NotFound<string>>(result);
    }

    [Fact]
    public async Task ShouldRejectRegistrationWhenNameIsEmpty()
    {
        // Arrange
        var context = CreateContext();

        context.User.Add(new User
        {
            Name = "Eduardo",
            Email = "eduardo@gmail.com",
            PasswordHash = "123456",
            Role = UserRole.Canteen
        });

        await context.SaveChangesAsync();

        var service = new SchoolService(context);

        var dto = new SchoolDto
        {
            Name = "",
            Address = "Rua A",
            Phone = "81986453728",
            Shifts = new List<Shift>
            {
                Shift.Morning
            }
        };

        // Act
        var result = await service.Register(dto, 1);

        // Assert
        Assert.IsType<BadRequest<string>>(result);
    }

    [Fact]
    public async Task ShouldRejectRegistrationWhenAddressIsEmpty()
    {
        // Arrange
        var context = CreateContext();

        context.User.Add(new User
        {
            Name = "Eduardo",
            Email = "eduardo@gmail.com",
            PasswordHash = "123456",
            Role = UserRole.Canteen
        });

        await context.SaveChangesAsync();

        var service = new SchoolService(context);

        var dto = new SchoolDto
        {
            Name = "ABC School",
            Address = "",
            Phone = "81986453728",
            Shifts = new List<Shift>
            {
                Shift.Morning
            }
        };

        // Act
        var result = await service.Register(dto, 1);

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
            Role = UserRole.Canteen
        });

        await context.SaveChangesAsync();

        var service = new SchoolService(context);

        var dto = new SchoolDto
        {
            Name = "ABC School",
            Address = "Main Street",
            Phone = "",
            Shifts = new List<Shift>
            {
                Shift.Morning
            }
        };

        // Act
        var result = await service.Register(dto, 1);

        // Assert
        Assert.IsType<BadRequest<string>>(result);
    }

    [Fact]
    public async Task ShouldRejectRegistrationWhenNoShiftIsProvided()
    {
        // Arrange
        var context = CreateContext();

        context.User.Add(new User
        {
            Name = "Eduardo",
            Email = "eduardo@gmail.com",
            PasswordHash = "123456",
            Role = UserRole.Canteen
        });

        await context.SaveChangesAsync();

        var service = new SchoolService(context);

        var dto = new SchoolDto
        {
            Name = "ABC School",
            Address = "Main Street",
            Phone = "11999999999",
            Shifts = new List<Shift>()
        };

        // Act
        var result = await service.Register(dto, 1);

        // Assert
        Assert.IsType<BadRequest<string>>(result);
    }
}