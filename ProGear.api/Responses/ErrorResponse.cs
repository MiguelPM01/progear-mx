namespace ProGear.Api;

public class ErrorResponse
{
    public bool Exito {get;set;}
    public required string  Codigo {get; set;}
    public required string Mensaje {get;set;}
}