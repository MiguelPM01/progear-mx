namespace ProGear.Api;
public class CrearProductoRequest
{
    public required string Nombre { get;set; }

    public required string Marca { get;set; }
    public required string Sku { get; set; }
    
}