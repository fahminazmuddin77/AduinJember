using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AduinJember.Configuration;
using AduinJember.Repositories;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AduinJember.Middleware;

public class JwtMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IAdminRepository adminRepo)
    {
        string? token = null;
        var authHeader = context.Request.Headers.Authorization.ToString();
        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            token = authHeader["Bearer ".Length..].Trim();
        }

        if (token != null)
            await AttachUserToContextAsync(context, token, adminRepo);

        await next(context);
    }

    private static async Task AttachUserToContextAsync(HttpContext context, string token, IAdminRepository adminRepo)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var email  = jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;

            var role = "user";
            if (Guid.TryParse(userId, out var parsedGuid))
            {
                var isAdmin = await adminRepo.IsAdminAsync(parsedGuid);
                if (isAdmin)
                {
                    role = "admin";
                }
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, userId ?? ""),
                new(ClaimTypes.Email, email ?? ""),
                new(ClaimTypes.Role, role)
            };

            context.User = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[JwtMiddleware] Token reading failed: {ex.Message}");
        }
    }
}
