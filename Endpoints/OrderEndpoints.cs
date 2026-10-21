using System.Security.Claims;
using cardapio_digital.Dtos;
using cardapio_digital.Services;

namespace cardapio_digital.Endpoints
{
    public static class OrderEndpoints
    {
        public static void MapOrderEndpoints(this WebApplication app)
        {
            // POST /orders  (responsável agenda o lanche de um filho)
            app.MapPost("/orders", async (OrderDto dto, HttpContext http, OrderService service) =>
            {
                var userId = int.Parse(http.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

                return await service.Create(dto, userId);
            })
            .RequireAuthorization("Parent");

            // GET /orders  ou  GET /orders?childId=1
            app.MapGet("/orders", async (int? childId, HttpContext http, OrderService service) =>
            {
                var userId = int.Parse(http.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

                return await service.GetAll(userId, childId);
            })
            .RequireAuthorization("Parent");
        }
    }
}
