using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Xunit;
using cardapio_digital;
using cardapio_digital.Dtos;
using cardapio_digital.Entities;
using cardapio_digital.Enums;
using cardapio_digital.Services;

namespace cardapio_digital.Tests.Orders;

public class OrderServiceTests
{
    private AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    // Cenário base:
    //  - responsável (user 1 / parent 1) com o filho 1 na escola 1
    //  - cardápio 1 da escola 1 com os produtos 1 (prato 8,50) e 2 (bebida 2,00)
    //  - produto 3 (bebida 3,50) existe na escola, mas NÃO está no cardápio
    private static async Task SeedAsync(AppDbContext context)
    {
        var parentUser = new User
        {
            Id = 1,
            Name = "Eduardo",
            Email = "eduardo@gmail.com",
            PasswordHash = "123456",
            Role = UserRole.Parent
        };

        var canteenUser = new User
        {
            Id = 2,
            Name = "Cantina",
            Email = "cantina@gmail.com",
            PasswordHash = "123456",
            Role = UserRole.Canteen
        };

        context.User.AddRange(parentUser, canteenUser);

        context.Schools.Add(new School
        {
            Id = 1,
            Name = "Colégio São José",
            Address = "Rua Pião, 112",
            Phone = "8133334444",
            Shifts = new List<Shift> { Shift.Evening },
            UserId = 2
        });

        context.Parents.Add(new Parent
        {
            Id = 1,
            Name = "Eduardo",
            Cpf = "12345678900",
            Phone = "81986453728",
            UserId = 1
        });

        context.Children.Add(new Child
        {
            Id = 1,
            Name = "Joao",
            BirthDate = new DateTime(2022, 11, 28),
            Phone = "82939903434",
            ParentId = 1,
            SchoolId = 1
        });

        context.Categories.AddRange(
            new Category { Id = 1, Name = "Pratos", SchoolId = 1 },
            new Category { Id = 2, Name = "Bebidas", SchoolId = 1 });

        context.Products.AddRange(
            new Product { Id = 1, Name = "Macarrão", Price = 8.5m, CategoryId = 1, SchoolId = 1 },
            new Product { Id = 2, Name = "Água", Price = 2m, CategoryId = 2, SchoolId = 1 },
            new Product { Id = 3, Name = "Suco", Price = 3.5m, CategoryId = 2, SchoolId = 1 });

        context.Menus.Add(new Menu { Id = 1, Name = "Cardápio Noite", SchoolId = 1 });

        context.MenuProducts.AddRange(
            new MenuProduct { MenuId = 1, ProductId = 1 },
            new MenuProduct { MenuId = 1, ProductId = 2 });

        await context.SaveChangesAsync();
    }

    private static OrderDto ValidDto() => new()
    {
        ChildId = 1,
        MenuId = 1,
        Date = DateTime.UtcNow.Date.AddDays(1),
        ProductIds = new List<int> { 1, 2 }
    };

    [Fact]
    public async Task ShouldCreateOrderWithItemsAndTotal()
    {
        // Arrange
        var context = CreateContext();
        await SeedAsync(context);
        var service = new OrderService(context);

        // Act
        var result = await service.Create(ValidDto(), 1);

        // Assert
        var created = Assert.IsType<Created<OrderResponseDto>>(result);
        Assert.Equal(10.5m, created.Value!.Total);
        Assert.Equal(2, created.Value.Items.Count);
        Assert.Equal("Joao", created.Value.ChildName);
        Assert.Equal("Cardápio Noite", created.Value.MenuName);
        Assert.Equal(1, await context.Orders.CountAsync());
        Assert.Equal(2, await context.OrderItems.CountAsync());
    }

    [Fact]
    public async Task ShouldRejectOrderWhenNoProductIsSelected()
    {
        // Arrange
        var context = CreateContext();
        var service = new OrderService(context);

        var dto = ValidDto();
        dto.ProductIds = new List<int>();

        // Act
        var result = await service.Create(dto, 1);

        // Assert
        Assert.IsType<BadRequest<string>>(result);
    }

    [Fact]
    public async Task ShouldRejectOrderWhenDateIsInThePast()
    {
        // Arrange
        var context = CreateContext();
        var service = new OrderService(context);

        var dto = ValidDto();
        dto.Date = DateTime.UtcNow.Date.AddDays(-5);

        // Act
        var result = await service.Create(dto, 1);

        // Assert
        Assert.IsType<BadRequest<string>>(result);
    }

    [Fact]
    public async Task ShouldRejectOrderWhenParentDoesNotExist()
    {
        // Arrange
        var context = CreateContext();
        await SeedAsync(context);
        var service = new OrderService(context);

        // Act
        var result = await service.Create(ValidDto(), 999);

        // Assert
        Assert.IsType<UnprocessableEntity<string>>(result);
    }

    [Fact]
    public async Task ShouldRejectOrderWhenChildBelongsToAnotherParent()
    {
        // Arrange
        var context = CreateContext();
        await SeedAsync(context);

        context.User.Add(new User
        {
            Id = 3,
            Name = "Outro",
            Email = "outro@gmail.com",
            PasswordHash = "123456",
            Role = UserRole.Parent
        });
        context.Parents.Add(new Parent
        {
            Id = 2,
            Name = "Outro",
            Cpf = "98765432100",
            Phone = "81900000000",
            UserId = 3
        });
        await context.SaveChangesAsync();

        var service = new OrderService(context);

        // Act (o filho 1 é do responsável 1, não do 2)
        var result = await service.Create(ValidDto(), 3);

        // Assert
        Assert.IsType<NotFound<string>>(result);
        Assert.Equal(0, await context.Orders.CountAsync());
    }

    [Fact]
    public async Task ShouldRejectOrderWhenMenuBelongsToAnotherSchool()
    {
        // Arrange
        var context = CreateContext();
        await SeedAsync(context);

        // Cada escola precisa de uma cantina (usuário) própria: a relação é 1:1
        context.User.Add(new User
        {
            Id = 4,
            Name = "Outra Cantina",
            Email = "outracantina@gmail.com",
            PasswordHash = "123456",
            Role = UserRole.Canteen
        });
        context.Schools.Add(new School
        {
            Id = 2,
            Name = "Outra Escola",
            Address = "Rua B, 20",
            Phone = "8144445555",
            Shifts = new List<Shift> { Shift.Morning },
            UserId = 4
        });
        context.Menus.Add(new Menu { Id = 2, Name = "Cardápio de outra escola", SchoolId = 2 });
        await context.SaveChangesAsync();

        var service = new OrderService(context);

        var dto = ValidDto();
        dto.MenuId = 2;

        // Act
        var result = await service.Create(dto, 1);

        // Assert
        Assert.IsType<BadRequest<string>>(result);
    }

    [Fact]
    public async Task ShouldRejectOrderWhenProductIsNotInTheMenu()
    {
        // Arrange
        var context = CreateContext();
        await SeedAsync(context);
        var service = new OrderService(context);

        var dto = ValidDto();
        dto.ProductIds = new List<int> { 1, 3 }; // 3 não está no cardápio

        // Act
        var result = await service.Create(dto, 1);

        // Assert
        Assert.IsType<BadRequest<string>>(result);
        Assert.Equal(0, await context.Orders.CountAsync());
    }

    [Fact]
    public async Task ShouldRejectDuplicateOrderForSameChildMenuAndDate()
    {
        // Arrange
        var context = CreateContext();
        await SeedAsync(context);
        var service = new OrderService(context);

        var dto = ValidDto();
        await service.Create(dto, 1);

        // Act
        var result = await service.Create(dto, 1);

        // Assert
        Assert.IsType<Conflict<string>>(result);
        Assert.Equal(1, await context.Orders.CountAsync());
    }

    [Fact]
    public async Task ShouldListOnlyOrdersFromTheLoggedParent()
    {
        // Arrange
        var context = CreateContext();
        await SeedAsync(context);

        context.User.Add(new User
        {
            Id = 3,
            Name = "Outro",
            Email = "outro@gmail.com",
            PasswordHash = "123456",
            Role = UserRole.Parent
        });
        context.Parents.Add(new Parent
        {
            Id = 2,
            Name = "Outro",
            Cpf = "98765432100",
            Phone = "81900000000",
            UserId = 3
        });
        await context.SaveChangesAsync();

        var service = new OrderService(context);
        await service.Create(ValidDto(), 1);

        // Act
        var mine = await service.GetAll(1, null);
        var others = await service.GetAll(3, null);

        // Assert
        Assert.Single(Assert.IsType<Ok<List<OrderResponseDto>>>(mine).Value!);
        Assert.Empty(Assert.IsType<Ok<List<OrderResponseDto>>>(others).Value!);
    }

    [Fact]
    public async Task ShouldReturnNotFoundWhenListingOrdersWithoutParent()
    {
        // Arrange
        var context = CreateContext();
        var service = new OrderService(context);

        // Act
        var result = await service.GetAll(999, null);

        // Assert
        Assert.IsType<NotFound<string>>(result);
    }
}
