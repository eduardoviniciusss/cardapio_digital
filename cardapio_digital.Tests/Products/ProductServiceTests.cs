using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Xunit;
using cardapio_digital;
using cardapio_digital.Dtos;
using cardapio_digital.Entities;
using cardapio_digital.Enums;
using cardapio_digital.Services;

namespace cardapio_digital.Tests.Products;

public class ProductServiceTests
{
    private AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task ShouldRejectRegistrationWhenCategoryDoesNotExist()
    {
        // Arrange
        var context = CreateContext();

        context.Schools.Add(new School
        {
            Id = 1,
            Name = "ABC",
            Address = "Rua A",
            Phone = "81986453728",
            Shifts = new List<Shift>(),
            UserId = 1
        });

        await context.SaveChangesAsync();

        var service = new ProductService(context);

        var dto = new ProductDto
        {
            Name = "Hamburger",
            Price = 15.00m,
            CategoryId = 999
        };

        // Act
        var result = await service.Register(dto, 1);

        // Assert
        Assert.IsType<BadRequest<string>>(result);
    }

    [Fact]
    public async Task ShouldRejectRegistrationWhenNameIsEmpty()
    {
        // Arrange
        var context = CreateContext();

        var service = new ProductService(context);

        var dto = new ProductDto
        {
            Name = "",
            Price = 15.00m,
            CategoryId = 1
        };

        // Act
        var result = await service.Register(dto, 1);

        // Assert
        Assert.IsType<BadRequest<string>>(result);
    }

    [Fact]
    public async Task ShouldRejectRegistrationWhenPriceIsLessThanOrEqualToZero()
    {
        // Arrange
        var context = CreateContext();

        var service = new ProductService(context);

        var dto = new ProductDto
        {
            Name = "Hamburger",
            Price = 0,
            CategoryId = 1
        };

        // Act
        var result = await service.Register(dto, 1);

        // Assert
        Assert.IsType<BadRequest<string>>(result);
    }

    [Fact]
    public async Task ShouldRejectRegistrationWhenCategoryIsInvalid()
    {
        // Arrange
        var context = CreateContext();

        context.Schools.Add(new School
        {
            Id = 1,
            Name = "ABC School",
            Address = "Main Street",
            Phone = "11999999999",
            Shifts = new List<Shift>(),
            UserId = 1
        });

        await context.SaveChangesAsync();

        var service = new ProductService(context);

        var dto = new ProductDto
        {
            Name = "Hamburger",
            Price = 15.00m,
            CategoryId = -1
        };

        // Act
        var result = await service.Register(dto, 1);

        // Assert
        Assert.IsType<BadRequest<string>>(result);
    }
}