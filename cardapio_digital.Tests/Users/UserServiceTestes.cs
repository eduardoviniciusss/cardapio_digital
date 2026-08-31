using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Configuration;
using Xunit;
using cardapio_digital;
using cardapio_digital.Dtos;
using cardapio_digital.Entities;
using cardapio_digital.Enums;
using cardapio_digital.Services;

namespace cardapio_digital.Tests.Users;

public class UserServiceTests
{
    private AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    private IConfiguration CreateConfiguration()
    {
        var inMemorySettings = new Dictionary<string, string?>
        {
            { "Jwt:SecretKey", "super_secret_key_1234567890123456" },
            { "Jwt:Issuer", "cardapio_digital" },
            { "Jwt:Audience", "cardapio_digital" },
            { "Jwt:ExpirationHours", "2" }
        };

        return new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();
    }

    [Fact]
    public async Task ShouldRejectRegistrationWhenEmailAlreadyExists()
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

        var userService = new UserService(context);

        var dto = new UserRegistrationDto
        {
            Name = "Carlos",
            Email = "eduardo@gmail.com",
            Password = "654321",
            Role = UserRole.Parent
        };

        // Act
        var result = await userService.Register(dto);

        // Assert
        Assert.IsType<BadRequest<string>>(result);
    }

    [Fact]
    public async Task ShouldEncryptPasswordWhenUserIsRegistered()
    {
        // Arrange
        var context = CreateContext();

        var service = new UserService(context);

        var dto = new UserRegistrationDto
        {
            Name = "Eduardo",
            Email = "eduardo@gmail.com",
            Password = "123456",
            Role = UserRole.Parent
        };

        // Act
        await service.Register(dto);

        var user = await context.User.FirstAsync();

        // Assert
        Assert.NotEqual("123456", user.PasswordHash);

        Assert.True(BCrypt.Net.BCrypt.Verify("123456", user.PasswordHash)
        );


    }

    [Fact]
    public async Task ShouldRejectRegistrationWhenProfileIsInvalid()
    {
        // Arrange
        var context = CreateContext();
        var service = new UserService(context);

        var dto = new UserRegistrationDto
        {
            Name = "Eduardo",
            Email = "eduardo@gmail.com",
            Password = "123456",
            Role = (UserRole)999
        };

        // Act
        var result = await service.Register(dto);

        // Assert
        Assert.IsType<BadRequest<string>>(result);
    }

    [Fact]
    public async Task ShouldLogInWhenEmailAndPasswordAreValid()
    {
        // Arrange
        var context = CreateContext();

        context.User.Add(new User
        {
            Name = "Eduardo",
            Email = "eduardo@gmail.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"),
            Role = UserRole.Parent
        });

        await context.SaveChangesAsync();

        var configuration = CreateConfiguration();
        var service = new LoginService(context, configuration);

        var dto = new UserLoginDto
        {
            Email = "eduardo@gmail.com",
            Password = "123456"
        };

        // Act
        var result = await service.Login(dto);

        // Assert
        Assert.IsNotType<BadRequest<string>>(result);
    }

    [Fact]
    public async Task ShouldDenyLoginWhenPasswordIsIncorrect()
    {
        // Arrange
        var context = CreateContext();

        context.User.Add(new User
        {
            Name = "Eduardo",
            Email = "eduardo@gmail.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"),
            Role = UserRole.Parent
        });

        await context.SaveChangesAsync();

        var configuration = CreateConfiguration();
        var service = new LoginService(context, configuration);

        var dto = new UserLoginDto
        {
            Email = "eduardo@gmail.com",
            Password = "654321"
        };

        // Act
        var result = await service.Login(dto);

        // Assert
        Assert.IsType<BadRequest<string>>(result);
    }

    [Fact]
    public async Task ShouldDenyLoginWhenEmailDoesNotExist()
    {
        // Arrange
       var context = CreateContext();

        var configuration = CreateConfiguration();
        var service = new LoginService(context, configuration);

        var dto = new UserLoginDto
        {
            Email = "naoexiste@gmail.com",
            Password = "123456"
        };

        // Act
        var result = await service.Login(dto);

        // Assert
        Assert.IsType<BadRequest<string>>(result);
    }

}