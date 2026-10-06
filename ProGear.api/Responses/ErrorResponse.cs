namespace ProGear.Api.Responses;

public class ErrorResponse
{
    public bool Exito {get;set;}
    public required string  Codigo {get; set;}
    public required string Mensaje {get;set;}
}