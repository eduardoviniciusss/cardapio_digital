using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Xunit;
using cardapio_digital;
using cardapio_digital.Dtos;
using cardapio_digital.Entities;
using cardapio_digital.Enums;
using cardapio_digital.Services;

namespace cardapio_digital.Tests.Categories;

public class CategoryServiceTests
{
    private AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task ShouldRejectRegistrationWhenSchoolDoesNotExist()
    {
        // Arrange
        var context = CreateContext();

        var service = new CategoryService(context);

        var dto = new CategoryDto
        {
            Name = "Snacks"
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

        context.Schools.Add(new School
        {
            Name = "ABC School",
            Address = "Rua A",
            Phone = "81986453728",
            Shifts = new List<Shift>(),
            UserId = 1
        });

        await context.SaveChangesAsync();

        var service = new CategoryService(context);

        var dto = new CategoryDto
        {
            Name = ""
        };

        // Act
        var result = await service.Register(dto, 1);

        // Assert
        Assert.IsType<BadRequest<string>>(result);
    }
}