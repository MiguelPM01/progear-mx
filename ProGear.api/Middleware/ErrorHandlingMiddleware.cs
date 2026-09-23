using System.Text.Json;

namespace ProGear.Api;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception)
        {
            context.Response.StatusCode = 400;
            context.Response.ContentType = "application/json";

            var error = new ErrorResponse
            {
                Exito = false,
                Codigo = "INVALID_REQUEST",
                Mensaje = "La solicitud contiene datos inválidos."
            };

            var json = JsonSerializer.Serialize(error);

            await context.Response.WriteAsync(json);
        }
    }
}

