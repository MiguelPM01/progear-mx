using ProGear.Api;
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var taladro = new Producto
{
    Nombre = "Taladro inalámbrico",
    Sku = "TAL-058",
    Precio = 1299.00m  
};

var esmeril = new Producto
{
    Nombre = "Esmeriladora Angular inalámbrica de 5 pulgadas",
    Sku = "ESM-215",
    Precio = 1741.00m
};

var llaveImpacto = new Producto
{
  Nombre = "Llave de impacto",
  Sku = "IMP-005",
  Precio = 1099.99m  
};

var multimetro = new Producto
{
    Nombre = "Multímetro digital",
    Sku = "MUL-895",
    Precio = 899.99m
};

var atornillador = new Producto
{
  Nombre = "Atornillador de impacto inalámbrico sin escobillas",
  Sku = "ATI-298",
  Precio = 999.99m  
};

var productos = new List<Producto>
{
    taladro,
    esmeril,
    llaveImpacto,
    multimetro,
    atornillador
};


foreach (var producto in productos)
{
    Console.WriteLine(producto.Nombre);
}

app.Run();

