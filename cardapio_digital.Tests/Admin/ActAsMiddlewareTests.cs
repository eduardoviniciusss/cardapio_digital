using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Xunit;
using cardapio_digital;
using cardapio_digital.Entities;
using cardapio_digital.Enums;
using cardapio_digital.Middleware;

namespace cardapio_digital.Tests.Admin;

public class ActAsMiddlewareTests
{
    private AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    // 1 = admin, 2 = cantina, 3 = responsável, 4 = outro admin
    private static async Task SeedAsync(AppDbContext context)
    {
        context.User.AddRange(
            new User { Id = 1, Name = "Admin", Email = "admin@gmail.com", PasswordHash = "123456", Role = UserRole.Administrator },
            new User { Id = 2, Name = "Cantina", Email = "cantina@gmail.com", PasswordHash = "123456", Role = UserRole.Canteen },
            new User { Id = 3, Name = "Responsável", Email = "pai@gmail.com", PasswordHash = "123456", Role = UserRole.Parent },
            new User { Id = 4, Name = "Outro Admin", Email = "admin2@gmail.com", PasswordHash = "123456", Role = UserRole.Administrator });

        await context.SaveChangesAsync();
    }

    private static DefaultHttpContext CreateHttp(string userId, string role, string? actAsHeader)
    {
        var http = new DefaultHttpContext();

        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Role, role)
        }, "test");

        http.User = new ClaimsPrincipal(identity);

        if (actAsHeader != null)
            http.Request.Headers[ActAsMiddleware.HeaderName] = actAsHeader;

        return http;
    }

    [Fact]
    public async Task AdminShouldActAsTheChosenUser()
    {
        // Arrange
        var context = CreateContext();
        await SeedAsync(context);
        var nextCalled = false;
        var middleware = new ActAsMiddleware(_ => { nextCalled = true; return Task.CompletedTask; });
        var http = CreateHttp("1", "Administrator", "2");

        // Act
        await middleware.InvokeAsync(http, context);

        // Assert
        Assert.True(nextCalled);
        Assert.Equal("2", http.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        Assert.True(http.User.IsInRole("Administrator"));
        Assert.Equal("1", http.User.FindFirst(ActAsMiddleware.ActingAdminClaim)!.Value);
    }

    [Fact]
    public async Task AdminWithoutHeaderShouldKeepOwnIdentity()
    {
        // Arrange
        var context = CreateContext();
        await SeedAsync(context);
        var nextCalled = false;
        var middleware = new ActAsMiddleware(_ => { nextCalled = true; return Task.CompletedTask; });
        var http = CreateHttp("1", "Administrator", null);

        // Act
        await middleware.InvokeAsync(http, context);

        // Assert
        Assert.True(nextCalled);
        Assert.Equal("1", http.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    }

    [Fact]
    public async Task NonAdminShouldHaveTheHeaderIgnored()
    {
        // Arrange
        var context = CreateContext();
        await SeedAsync(context);
        var nextCalled = false;
        var middleware = new ActAsMiddleware(_ => { nextCalled = true; return Task.CompletedTask; });
        var http = CreateHttp("2", "Canteen", "3");

        // Act
        await middleware.InvokeAsync(http, context);

        // Assert
        Assert.True(nextCalled);
        Assert.Equal("2", http.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    }

    [Fact]
    public async Task ShouldRejectInvalidHeaderValue()
    {
        // Arrange
        var context = CreateContext();
        await SeedAsync(context);
        var nextCalled = false;
        var middleware = new ActAsMiddleware(_ => { nextCalled = true; return Task.CompletedTask; });
        var http = CreateHttp("1", "Administrator", "abc");

        // Act
        await middleware.InvokeAsync(http, context);

        // Assert
        Assert.False(nextCalled);
        Assert.Equal(StatusCodes.Status400BadRequest, http.Response.StatusCode);
    }

    [Fact]
    public async Task ShouldRejectUnknownUser()
    {
        // Arrange
        var context = CreateContext();
        await SeedAsync(context);
        var nextCalled = false;
        var middleware = new ActAsMiddleware(_ => { nextCalled = true; return Task.CompletedTask; });
        var http = CreateHttp("1", "Administrator", "999");

        // Act
        await middleware.InvokeAsync(http, context);

        // Assert
        Assert.False(nextCalled);
        Assert.Equal(StatusCodes.Status404NotFound, http.Response.StatusCode);
    }

    [Fact]
    public async Task ShouldNotAllowActingAsAnotherAdministrator()
    {
        // Arrange
        var context = CreateContext();
        await SeedAsync(context);
        var nextCalled = false;
        var middleware = new ActAsMiddleware(_ => { nextCalled = true; return Task.CompletedTask; });
        var http = CreateHttp("1", "Administrator", "4");

        // Act
        await middleware.InvokeAsync(http, context);

        // Assert
        Assert.False(nextCalled);
        Assert.Equal(StatusCodes.Status404NotFound, http.Response.StatusCode);
    }
}
