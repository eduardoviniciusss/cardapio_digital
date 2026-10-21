using Microsoft.EntityFrameworkCore;
using cardapio_digital.Dtos;
using cardapio_digital.Entities;

namespace cardapio_digital.Services;

public class OrderService
{
    private readonly AppDbContext _db;

    public OrderService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IResult> Create(OrderDto dto, int userId)
    {
        if (dto.ChildId <= 0)
            return Results.BadRequest("Invalid child.");

        if (dto.MenuId <= 0)
            return Results.BadRequest("Invalid menu.");

        if (dto.Date == default)
            return Results.BadRequest("Date is required.");

        var productIds = (dto.ProductIds ?? new List<int>()).Distinct().ToList();

        if (productIds.Count == 0)
            return Results.BadRequest("Select at least one product.");

        if (productIds.Any(id => id <= 0))
            return Results.BadRequest("Invalid product.");

        // Aceita "ontem" em UTC para não recusar quem está no Brasil (UTC-3)
        // no fim do dia, quando o relógio UTC já virou para o dia seguinte.
        if (dto.Date.Date < DateTime.UtcNow.Date.AddDays(-1))
            return Results.BadRequest("Date cannot be in the past.");

        var parent = await _db.Parents.FirstOrDefaultAsync(p => p.UserId == userId);

        if (parent == null)
            return Results.UnprocessableEntity("Parent not found.");

        // Só aceita filhos do próprio responsável logado
        var child = await _db.Children
            .FirstOrDefaultAsync(c => c.Id == dto.ChildId && c.ParentId == parent.Id);

        if (child == null)
            return Results.NotFound("Child not found.");

        var menu = await _db.Menus.FirstOrDefaultAsync(m => m.Id == dto.MenuId);

        if (menu == null)
            return Results.NotFound("Menu not found.");

        if (menu.SchoolId != child.SchoolId)
            return Results.BadRequest("Menu belongs to another school.");

        // Todos os produtos escolhidos precisam estar mesmo neste cardápio
        var menuProducts = await _db.MenuProducts
            .Include(mp => mp.Product).ThenInclude(p => p.Category)
            .Where(mp => mp.MenuId == menu.Id && productIds.Contains(mp.ProductId))
            .ToListAsync();

        if (menuProducts.Count != productIds.Count)
            return Results.BadRequest("One or more products are not in this menu.");

        var date = dto.Date.Date;

        var alreadyExists = await _db.Orders.AnyAsync(o =>
            o.ChildId == child.Id &&
            o.MenuId == menu.Id &&
            o.Date == date);

        if (alreadyExists)
            return Results.Conflict("This child already has an order for this menu on this date.");

        var order = new Order
        {
            ChildId = child.Id,
            Child = child,
            MenuId = menu.Id,
            MenuName = menu.Name,
            Date = date,
            CreatedAt = DateTime.UtcNow,
            Items = menuProducts.Select(mp => new OrderItem
            {
                ProductId = mp.ProductId,
                ProductName = mp.Product.Name,
                CategoryName = mp.Product.Category?.Name ?? "No Category",
                UnitPrice = mp.Product.Price
            }).ToList()
        };

        order.Total = order.Items.Sum(i => i.UnitPrice);

        _db.Orders.Add(order);
        await _db.SaveChangesAsync();

        return Results.Created($"/orders/{order.Id}", ToResponse(order));
    }

    // Lista os agendamentos do responsável logado (opcionalmente de um filho só)
    public async Task<IResult> GetAll(int userId, int? childId)
    {
        var parent = await _db.Parents.FirstOrDefaultAsync(p => p.UserId == userId);

        if (parent == null)
            return Results.NotFound("Parent not found.");

        var query = _db.Orders
            .Include(o => o.Child)
            .Include(o => o.Items)
            .Where(o => o.Child.ParentId == parent.Id);

        if (childId.HasValue)
            query = query.Where(o => o.ChildId == childId.Value);

        var orders = await query
            .OrderByDescending(o => o.Date)
            .ThenByDescending(o => o.Id)
            .ToListAsync();

        return Results.Ok(orders.Select(ToResponse).ToList());
    }

    private static OrderResponseDto ToResponse(Order o) => new()
    {
        Id = o.Id,
        ChildId = o.ChildId,
        ChildName = o.Child.Name,
        MenuId = o.MenuId,
        MenuName = o.MenuName,
        Date = o.Date,
        Total = o.Total,
        CreatedAt = o.CreatedAt,
        Items = o.Items.Select(i => new OrderItemResponseDto
        {
            ProductId = i.ProductId,
            Name = i.ProductName,
            Category = i.CategoryName,
            Price = i.UnitPrice
        }).ToList()
    };
}
