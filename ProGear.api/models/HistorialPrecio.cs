namespace ProGear.Api.Models;

public class HistorialPrecio
{
    public int Id { get;set; }

    public int IdProducto { get; set; }

    public decimal Precio { get;set; }

    public DateTime FechaInicio { get;set; }

    public DateTime? FechaFin { get;set; }

}