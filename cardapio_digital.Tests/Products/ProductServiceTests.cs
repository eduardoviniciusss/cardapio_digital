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

    [Fact]
    public async Task ShouldRegisterProductSuccessfully()
    {
        // Arrange
        var context = CreateContext();

        context.Schools.Add(new School
        {
            Id = 1,
            Name = "ABC School",
            Address = "Main Street",
            Phone = "81999999999",
            Shifts = new List<Shift>(),
            UserId = 1
        });

        context.Categories.Add(new Category
        {
            Id = 1,
            Name = "Snacks",
            SchoolId = 1
        });

        await context.SaveChangesAsync();

        var service = new ProductService(context);

        var dto = new ProductDto
        {
            Name = "Hamburger",
            Price = 15.00m,
            CategoryId = 1
        };

        // Act
        var result = await service.Register(dto, 1);

        // Assert
        var created = Assert.IsType<Created<ProductResponseDto>>(result);
    }

    [Fact]
public async Task ShouldRejectRegistrationWhenSchoolDoesNotExist()
{
    // Arrange
    var context = CreateContext();

    context.Categories.Add(new Category
    {
        Id = 1,
        Name = "Snacks"
    });

    await context.SaveChangesAsync();

    var service = new ProductService(context);

    var dto = new ProductDto
    {
        Name = "Hamburger",
        Price = 15,
        CategoryId = 1
    };

    // Act
    var result = await service.Register(dto, 1);

    // Assert
    Assert.IsType<NotFound<string>>(result);
}

[Fact]
public async Task ShouldUpdateProductSuccessfully()
{
    var context = CreateContext();

    context.Schools.Add(new School
    {
        Id = 1,
        UserId = 1,
        Name = "ABC",
        Address = "Rua A",
        Phone = "81999999999",
        Shifts = new List<Shift>()
    });

    context.Categories.Add(new Category
    {
        Id = 1,
        Name = "Snacks",
        SchoolId = 1
    });

    context.Products.Add(new Product
    {
        Id = 1,
        Name = "Old Name",
        Price = 10,
        CategoryId = 1,
        SchoolId = 1
    });

    await context.SaveChangesAsync();

    var service = new ProductService(context);

    var dto = new ProductDto
    {
        Name = "New Name",
        Price = 25,
        CategoryId = 1
    };

    var result = await service.Update(1, dto, 1);

    var ok = Assert.IsType<Ok<ProductResponseDto>>(result);
}
[Fact]
public async Task ShouldDeleteProductSuccessfully()
{
    var context = CreateContext();

    context.Schools.Add(new School
    {
        Id = 1,
        UserId = 1,
        Name = "ABC",
        Address = "Rua A",
        Phone = "81999999999",
        Shifts = new List<Shift>()
    });

    context.Categories.Add(new Category
    {
        Id = 1,
        Name = "Snacks",
        SchoolId = 1
    });

    context.Products.Add(new Product
    {
        Id = 1,
        Name = "Hamburger",
        Price = 20,
        CategoryId = 1,
        SchoolId = 1
    });

    await context.SaveChangesAsync();

    var service = new ProductService(context);

    var result = await service.Delete(1, 1);

    Assert.IsType<NoContent>(result);

    Assert.Empty(context.Products);
}

[Fact]
public async Task ShouldReturnNotFoundWhenSchoolDoesNotExist()
{
    // Arrange
    var context = CreateContext();

    var service = new ProductService(context);

    // Act
    var result = await service.GetAll(1);

    // Assert
    Assert.IsType<NotFound<string>>(result);
}

[Fact]
public async Task ShouldReturnProductByIdSuccessfully()
{
    // Arrange
    var context = CreateContext();

    context.Schools.Add(new School
    {
        Id = 1,
        UserId = 1,
        Name = "ABC School",
        Address = "Rua A",
        Phone = "81999999999",
        Shifts = new List<Shift>()
    });

    context.Categories.Add(new Category
    {
        Id = 1,
        Name = "Snacks",
        SchoolId = 1
    });

    context.Products.Add(new Product
    {
        Id = 1,
        Name = "Hamburger",
        Price = 18.50m,
        CategoryId = 1,
        SchoolId = 1
    });

    await context.SaveChangesAsync();

    var service = new ProductService(context);

    // Act
    var result = await service.GetById(1, 1);

    // Assert
    var okResult = Assert.IsType<Ok<ProductResponseDto>>(result);
}

[Fact]
public async Task ShouldReturnNotFoundWhenProductDoesNotExist()
{
    // Arrange
    var context = CreateContext();

    context.Schools.Add(new School
    {
        Id = 1,
        UserId = 1,
        Name = "ABC School",
        Address = "Rua A",
        Phone = "81999999999",
        Shifts = new List<Shift>()
    });

    await context.SaveChangesAsync();

    var service = new ProductService(context);

    // Act
    var result = await service.GetById(1, 1);

    // Assert
    Assert.IsType<NotFound<string>>(result);
}

[Fact]
public async Task ShouldReturnAllProductsSuccessfully()
{
    // Arrange
    var context = CreateContext();

    context.Schools.Add(new School
    {
        Id = 1,
        UserId = 1,
        Name = "ABC School",
        Address = "Rua A",
        Phone = "81999999999",
        Shifts = new List<Shift>()
    });

    context.Categories.Add(new Category
    {
        Id = 1,
        Name = "Snacks",
        SchoolId = 1
    });

    context.Products.Add(new Product
    {
        Id = 1,
        Name = "Hamburger",
        Price = 20,
        CategoryId = 1,
        SchoolId = 1
    });

    await context.SaveChangesAsync();

    var service = new ProductService(context);

    // Act
    var result = await service.GetAll(1);

    // Assert
    var ok = Assert.IsType<Ok<List<ProductResponseDto>>>(result);

}
[Fact]
public async Task ShouldReturnEmptyListWhenSchoolHasNoProducts()
{
    // Arrange
    var context = CreateContext();

    context.Schools.Add(new School
    {
        Id = 1,
        UserId = 1,
        Name = "ABC School",
        Address = "Rua A",
        Phone = "81999999999",
        Shifts = new List<Shift>()
    });

    await context.SaveChangesAsync();

    var service = new ProductService(context);

    // Act
    var result = await service.GetAll(1);

    // Assert
    var ok = Assert.IsType<Ok<List<ProductResponseDto>>>(result);
}

[Fact]
public async Task ShouldReturnNotFoundWhenUpdatingProductDoesNotExist()
{
    // Arrange
    var context = CreateContext();

    context.Schools.Add(new School
    {
        Id = 1,
        UserId = 1,
        Name = "ABC School",
        Address = "Rua A",
        Phone = "81999999999",
        Shifts = new List<Shift>()
    });

    await context.SaveChangesAsync();

    var service = new ProductService(context);

    var dto = new ProductDto
    {
        Name = "New Product",
        Price = 20,
        CategoryId = 1
    };

    // Act
    var result = await service.Update(1, dto, 1);

    // Assert
    Assert.IsType<NotFound<string>>(result);
}
[Fact]
public async Task ShouldRejectUpdateWhenNameIsEmpty()
{
    // Arrange
    var context = CreateContext();

    context.Schools.Add(new School
    {
        Id = 1,
        UserId = 1,
        Name = "ABC School",
        Address = "Rua A",
        Phone = "81999999999",
        Shifts = new List<Shift>()
    });

    context.Categories.Add(new Category
    {
        Id = 1,
        Name = "Snacks",
        SchoolId = 1
    });

    context.Products.Add(new Product
    {
        Id = 1,
        Name = "Hamburger",
        Price = 20,
        CategoryId = 1,
        SchoolId = 1
    });

    await context.SaveChangesAsync();

    var service = new ProductService(context);

    var dto = new ProductDto
    {
        Name = "",
        Price = 20,
        CategoryId = 1
    };

    // Act
    var result = await service.Update(1, dto, 1);

    // Assert
    Assert.IsType<BadRequest<string>>(result);
}

[Fact]
public async Task ShouldReturnNotFoundWhenDeletingWithoutSchool()
{
    // Arrange
    var context = CreateContext();

    var service = new ProductService(context);

    // Act
    var result = await service.Delete(1, 1);

    // Assert
    Assert.IsType<NotFound<string>>(result);
}

[Fact]
public async Task ShouldReturnNotFoundWhenGettingProductByIdWithoutSchool()
{
    // Arrange
    var context = CreateContext();

    var service = new ProductService(context);

    // Act
    var result = await service.GetById(1, 1);

    // Assert
    Assert.IsType<NotFound<string>>(result);
}
[Fact]
public async Task ShouldReturnNotFoundWhenUpdatingWithoutSchool()
{
    // Arrange
    var context = CreateContext();

    var service = new ProductService(context);

    var dto = new ProductDto
    {
        Name = "Hamburger",
        Price = 20,
        CategoryId = 1
    };

    // Act
    var result = await service.Update(1, dto, 1);

    // Assert
    Assert.IsType<NotFound<string>>(result);
}

[Fact]
public async Task ShouldRejectUpdateWhenCategoryDoesNotExist()
{
    // Arrange
    var context = CreateContext();

    context.Schools.Add(new School
    {
        Id = 1,
        UserId = 1,
        Name = "ABC School",
        Address = "Rua A",
        Phone = "81999999999",
        Shifts = new List<Shift>()
    });

    context.Products.Add(new Product
    {
        Id = 1,
        Name = "Hamburger",
        Price = 20,
        CategoryId = 1,
        SchoolId = 1
    });

    await context.SaveChangesAsync();

    var service = new ProductService(context);

    var dto = new ProductDto
    {
        Name = "Novo Hambúrguer",
        Price = 25,
        CategoryId = 999
    };

    // Act
    var result = await service.Update(1, dto, 1);

    // Assert
    Assert.IsType<BadRequest<string>>(result);
}

[Fact]
public async Task ShouldReturnNotFoundWhenDeletingProductDoesNotExist()
{
    // Arrange
    var context = CreateContext();

    context.Schools.Add(new School
    {
        Id = 1,
        UserId = 1,
        Name = "ABC School",
        Address = "Rua A",
        Phone = "81999999999",
        Shifts = new List<Shift>()
    });

    await context.SaveChangesAsync();

    var service = new ProductService(context);

    // Act
    var result = await service.Delete(999, 1);

    // Assert
    Assert.IsType<NotFound<string>>(result);
}

}