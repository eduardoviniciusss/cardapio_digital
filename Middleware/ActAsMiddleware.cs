using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using cardapio_digital.Enums;

namespace cardapio_digital.Middleware;

public class ActAsMiddleware
{
    public const string HeaderName = "X-Act-As-User";
    public const string ActingAdminClaim = "acting_admin_id";

    private readonly RequestDelegate _next;

    public ActAsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, AppDbContext db)
    {
        var isAdmin = context.User.Identity?.IsAuthenticated == true
            && context.User.IsInRole(UserRole.Administrator.ToString());

        if (!isAdmin || !context.Request.Headers.TryGetValue(HeaderName, out var rawValue))
        {
            await _next(context);
            return;
        }

        if (!int.TryParse(rawValue.ToString(), out var targetUserId))
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync($"Invalid {HeaderName} header.");
            return;
        }

        var target = await db.User
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == targetUserId);

        if (target == null || target.Role == UserRole.Administrator)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsJsonAsync("User to act as was not found.");
            return;
        }

        var adminId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var claims = context.User.Claims
            .Where(c => c.Type != ClaimTypes.NameIdentifier)
            .ToList();

        claims.Add(new Claim(ClaimTypes.NameIdentifier, target.Id.ToString()));

        if (adminId != null)
            claims.Add(new Claim(ActingAdminClaim, adminId));

        context.User = new ClaimsPrincipal(
            new ClaimsIdentity(claims, context.User.Identity!.AuthenticationType));

        await _next(context);
    }
}
