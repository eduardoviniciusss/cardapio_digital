using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Xunit;
using cardapio_digital;
using cardapio_digital.Dtos;
using cardapio_digital.Entities;
using cardapio_digital.Enums;
using cardapio_digital.Services;

namespace cardapio_digital.Tests.Menus;

public class MenuServiceTests
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

        var service = new MenuService(context);

        var dto = new MenuDto
        {
            Name = "Monday Menu"
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
            Address = "Main Street",
            Phone = "11999999999",
            Shifts = new List<Shift>(),
            UserId = 1
        });

        await context.SaveChangesAsync();

        var service = new MenuService(context);

        var dto = new MenuDto
        {
            Name = ""
        };

        // Act
        var result = await service.Register(dto, 1);

        // Assert
        Assert.IsType<BadRequest<string>>(result);
    }

    [Fact]
    public async Task ShouldRejectAddingProductWhenProductDoesNotExist()
    {
        // Arrange
        var context = CreateContext();

        context.Schools.Add(new School { Id = 1, Name = "Real", Address = "Rua A", Phone = "8198471298", Shifts = new(), UserId = 1 });
        context.Menus.Add(new Menu { Id = 1, Name = "Suco", SchoolId = 1 });
        await context.SaveChangesAsync();

        var service = new MenuService(context);

        // Act
        var result = await service.AddProduct(1, 999);

        // Assert
        Assert.IsType<BadRequest<string>>(result);
    }

    [Fact]
    public async Task ShouldRejectAddingProductWhenProductBelongsToAnotherSchool()
    {
        // Arrange
        var context = CreateContext();

        context.Schools.Add(new School { Id = 1, Name = "Real", Address = "Rua A", Phone = "8198471298", Shifts = new(), UserId = 1 });
        context.Schools.Add(new School { Id = 2, Name = "Csj", Address = "Rua B", Phone = "8198471289", Shifts = new(), UserId = 2 });

        context.Categories.Add(new Category { Id = 1, Name = "Salgados", SchoolId = 2 });
        context.Menus.Add(new Menu { Id = 1, Name = "Noite", SchoolId = 1 });
        context.Products.Add(new Product { Id = 2, Name = "Pão", Price = 10, CategoryId = 1, SchoolId = 2 });

        await context.SaveChangesAsync();

        var service = new MenuService(context);

        // Act
        var result = await service.AddProduct(1, 2);

        // Assert
        Assert.IsType<BadRequest<string>>(result);
    }

    [Fact]
    public async Task ShouldRejectAddingProductWhenProductAlreadyExistsInMenu()
    {
        // Arrange
        var context = CreateContext();

        context.Schools.Add(new School { Id = 1, Name = "Real", Address = "Rua A", Phone = "8198471298", Shifts = new(), UserId = 1 });
        context.Categories.Add(new Category { Id = 1, Name = "Salgados", SchoolId = 1 });
        context.Menus.Add(new Menu { Id = 1, Name = "Noite", SchoolId = 1 });
        context.Products.Add(new Product { Id = 1, Name = "Pão", Price = 10, CategoryId = 1, SchoolId = 1 });
        context.MenuProducts.Add(new MenuProduct { MenuId = 1, ProductId = 1 });

        await context.SaveChangesAsync();

        var service = new MenuService(context);

        // Act
        var result = await service.AddProduct(1, 1);

        // Assert
        Assert.IsType<BadRequest<string>>(result);
    }

    [Fact]
public async Task ShouldRegisterMenuSuccessfully()
{
    // Arrange
    var context = CreateContext();

    context.Schools.Add(new School
    {
        Id = 1,
        Name = "ABC School",
        Address = "Street",
        Phone = "999999",
        UserId = 1,
        Shifts = new()
    });

    await context.SaveChangesAsync();

    var service = new MenuService(context);

    var dto = new MenuDto
    {
        Name = "Monday Menu"
    };

    // Act
    var result = await service.Register(dto, 1);

    // Assert
    var created = Assert.IsType<Created<MenuResponseDto>>(result);
}

[Fact]
public async Task ShouldReturnNotFoundWhenSchoolDoesNotExistOnGetAll()
{
    var context = CreateContext();

    var service = new MenuService(context);

    var result = await service.GetAll(999);

    Assert.IsType<NotFound<string>>(result);
}

[Fact]
public async Task ShouldReturnMenusSuccessfully()
{
    var context = CreateContext();

    context.Schools.Add(new School
    {
        Id = 1,
        Name = "School",
        Address = "Street",
        Phone = "999",
        UserId = 1,
        Shifts = new()
    });

    context.Menus.Add(new Menu
    {
        Id = 1,
        Name = "Breakfast",
        SchoolId = 1
    });

    context.Menus.Add(new Menu
    {
        Id = 2,
        Name = "Lunch",
        SchoolId = 1
    });

    await context.SaveChangesAsync();

    var service = new MenuService(context);

    var result = await service.GetAll(1);

    var ok = Assert.IsType<Ok<List<MenuResponseDto>>>(result);
}

[Fact]
public async Task ShouldRejectAddingProductWhenMenuDoesNotExist()
{
    var context = CreateContext();

    var service = new MenuService(context);

    var result = await service.AddProduct(999, 1);

    Assert.IsType<BadRequest<string>>(result);
}

[Fact]
public async Task ShouldAddProductToMenuSuccessfully()
{
    var context = CreateContext();

    context.Schools.Add(new School
    {
        Id = 1,
        Name = "School",
        Address = "Street",
        Phone = "999",
        UserId = 1,
        Shifts = new()
    });

    context.Categories.Add(new Category
    {
        Id = 1,
        Name = "Snacks",
        SchoolId = 1
    });

    context.Menus.Add(new Menu
    {
        Id = 1,
        Name = "Breakfast",
        SchoolId = 1
    });

    context.Products.Add(new Product
    {
        Id = 1,
        Name = "Bread",
        Price = 5,
        CategoryId = 1,
        SchoolId = 1
    });

    await context.SaveChangesAsync();

    var service = new MenuService(context);

    var result = await service.AddProduct(1, 1);

    var created = Assert.IsType<Created<MenuProductResponseDto>>(result);
}
}