using System.Security.Claims;
using cardapio_digital.Dtos;
using cardapio_digital.Services;

namespace cardapio_digital.Endpoints
{
    public static class OrderEndpoints
    {
        public static void MapOrderEndpoints(this WebApplication app)
        {
            // POST 
            app.MapPost("/orders", async (OrderDto dto, HttpContext http, OrderService service) =>
            {
                var userId = int.Parse(http.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

                return await service.Create(dto, userId);
            })
            .RequireAuthorization("Parent");

            // GET 
            app.MapGet("/orders", async (int? childId, HttpContext http, OrderService service) =>
            {
                var userId = int.Parse(http.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

                return await service.GetAll(userId, childId);
            })
            .RequireAuthorization("Parent");

            // DELETE
            app.MapDelete("/orders/{id}", async (int id, HttpContext http, OrderService service) =>
            {
                var userId = int.Parse(http.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

                return await service.Cancel(id, userId);
            })
            .RequireAuthorization("Parent");

            // GET /orders/school 
            app.MapGet("/orders/school", async (DateTime? date, HttpContext http, OrderService service) =>
            {
                var userId = int.Parse(http.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

                return await service.GetForSchool(userId, date);
            })
            .RequireAuthorization("Canteen");
        }
    }
}
