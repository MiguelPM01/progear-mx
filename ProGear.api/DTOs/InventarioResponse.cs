namespace ProGear.Api.DTOs;

public class InventarioResponse
{
    public int IdProducto { get;set; }
    public required string Nombre { get;set;}
    public required string Marca { get;set; }
    public required string Sku { get;set; }
    public int Existencia { get;set; }
    public int Reservado { get;set; }
    public int Disponible { get;set; }
}

