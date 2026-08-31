using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Xunit;
using cardapio_digital;
using cardapio_digital.Dtos;
using cardapio_digital.Entities;
using cardapio_digital.Enums;
using cardapio_digital.Services;

namespace cardapio_digital.Tests.Children;

public class ChildServiceTests
{
    private AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task ShouldRejectRegistrationWhenParentDoesNotExist()
    {
        // Arrange
        var context = CreateContext();

        var service = new ChildService(context);

        var dto = new ChildRegistrationDto
        {
            Name = "Pedro",
            BirthDate = new DateTime(2018, 5, 10),
            Phone = "81986453728",
            SchoolId = 1
        };

        // Act
        var result = await service.Register(dto, 999);

        // Assert
        Assert.IsType<UnprocessableEntity<string>>(result);
    }

    [Fact]
    public async Task ShouldRejectRegistrationWhenSchoolDoesNotExist()
    {
        // Arrange
        var context = CreateContext();

        var user = new User
        {
            Id = 1,
            Name = "Eduardo",
            Email = "eduardo@gmail.com",
            PasswordHash = "123456",
            Role = UserRole.Parent
        };

        context.User.Add(user);

        context.Parents.Add(new Parent
        {
            Id = 1,
            Cpf = "12345678900",
            Phone = "81986453728",
            UserId = 1,
            User = user
        });

        await context.SaveChangesAsync();

        var service = new ChildService(context);

        var dto = new ChildRegistrationDto
        {
            Name = "Pedro",
            BirthDate = new DateTime(2018, 5, 10),
            Phone = "81986453728",
            SchoolId = 999
        };

        // Act
        var result = await service.Register(dto, 1);

        // Assert
        Assert.IsType<UnprocessableEntity<string>>(result);
    }

    [Fact]
    public async Task ShouldRejectRegistrationWhenNameIsEmpty()
    {
        // Arrange
        var context = CreateContext();

        var service = new ChildService(context);

        var dto = new ChildRegistrationDto
        {
            Name = "",
            BirthDate = new DateTime(2018, 5, 10),
            Phone = "81986453728",
            SchoolId = 1
        };

        // Act
        var result = await service.Register(dto, 1);

        // Assert
        Assert.IsType<BadRequest<string>>(result);
    }

    [Fact]
    public async Task ShouldRejectRegistrationWhenBirthDateIsNotProvided()
    {
        // Arrange
        var context = CreateContext();

        var service = new ChildService(context);

        var dto = new ChildRegistrationDto
        {
            Name = "Pedro",
            BirthDate = default,
            Phone = "81986453728",
            SchoolId = 1
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

        var service = new ChildService(context);

        var dto = new ChildRegistrationDto
        {
            Name = "Pedro",
            BirthDate = new DateTime(2018, 5, 10),
            Phone = "",
            SchoolId = 1
        };

        // Act
        var result = await service.Register(dto, 1);

        // Assert
        Assert.IsType<BadRequest<string>>(result);
    }
}