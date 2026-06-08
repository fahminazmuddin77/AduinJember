using System.Net;
using System.Text.Json;

namespace AduinJember.Middleware;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, message) = ex switch
        {
            UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "Akses ditolak."),
            KeyNotFoundException        => (HttpStatusCode.NotFound, ex.Message),
            ArgumentException           => (HttpStatusCode.BadRequest, ex.Message),
            _                           => (HttpStatusCode.InternalServerError, "Terjadi kesalahan pada server.")
        };

        context.Response.StatusCode = (int)statusCode;

        var result = JsonSerializer.Serialize(new
        {
            success = false,
            message,
            statusCode = (int)statusCode
        });

        return context.Response.WriteAsync(result);
    }
}
