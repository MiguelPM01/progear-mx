using System.Text.Json;
using ProGear.Api.Responses;

namespace ProGear.Api.Middleware;


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
        catch (BadHttpRequestException ex)
        {
            await WriteErrorResponse(
                context,
                ex.StatusCode,
                "INVALID_REQUEST",
                "La solicitud contiene datos inválidos."
            );
        }
        catch (Exception)
        {
            await WriteErrorResponse(
                context,
                StatusCodes.Status500InternalServerError,
                "INTERNAL_SERVER_ERROR",
                "Ocurrió un error interno en el servidor."
            );
        }
    }

    private static async Task WriteErrorResponse(
        HttpContext context,
        int StatusCode,
        string codigo,
        string mensaje)
    {
        context.Response.StatusCode = StatusCode;
        context.Response.ContentType = "application/json";

        var error = new ErrorResponse
        {
            Exito = false,
            Codigo = codigo,
            Mensaje = mensaje
        };

        var json = JsonSerializer.Serialize(error);

        await context.Response.WriteAsync(json);
    }
}