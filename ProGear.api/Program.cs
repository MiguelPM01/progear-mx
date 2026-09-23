using ProGear.Api;
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

var taladro = new Producto
{
    Id = 1,
    Nombre = "Taladro inalámbrico",
    Sku = "TAL-058",
    Precio = 1299.00m  
};

var esmeril = new Producto
{
    Id = 2,
    Nombre = "Esmeriladora Angular inalámbrica de 5 pulgadas",
    Sku = "ESM-215",
    Precio = 1741.00m
};

var llaveImpacto = new Producto
{
    Id = 3,
  Nombre = "Llave de impacto",
  Sku = "IMP-005",
  Precio = 1099.99m  
};

var multimetro = new Producto
{
    Id = 4,
    Nombre = "Multímetro digital",
    Sku = "MUL-895",
    Precio = 899.99m
};

var atornillador = new Producto
{
    Id = 5,
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


    app.MapGet("/productos", () =>
    {
        return productos;
    });


    app.MapGet("/productos/{id}", (int id) =>
    {
        if (id <= 0)
            {
                return Results.BadRequest(new ErrorResponse
                {
                    Exito = false,
                    Codigo = "INVALID_PRODUCT_ID",
                    Mensaje = "El identificador del producto debe ser mayor que cero."
                });
            }
        var producto = productos.Find(p => p.Id == id);

        if(producto != null)
        {
            return Results.Ok(producto);
        }
        else
        {
            return Results.NotFound( new ErrorResponse
            {
                Exito = false,
                Codigo = "PRODUCT_NOT_fOUND",
                Mensaje = "No encontramos un producto con el identificador proporcionado."
            });
        }
    });

    int siguienteId = 6;

    app.MapPost("/productos", (CrearProductoRequest request) =>
    {
        if (string.IsNullOrWhiteSpace(request.Nombre))
        {
            return Results.BadRequest(new ErrorResponse
            {
                Exito = false,
                Codigo = "INVALID_PRODUCT_NAME",
                Mensaje = "El nombre del producto no puede estar vacío."
            });
        }

        if (string.IsNullOrWhiteSpace(request.Sku))
        {
            return Results.BadRequest(new ErrorResponse
            {
                Exito = false,
                Codigo = "INVALID_PRODUCT_SKU",
                Mensaje = "El SKU del producto no puede estar vacío."
            });
        }

        if (productos.Exists(p => p.Sku == request.Sku))
        {
            return Results.Conflict(new ErrorResponse
            {
               Exito = false,
               Codigo = "DUPLICATED_SKU",
               Mensaje = "El SKU del producto ya está registrado." 
            });
        }

        if (request.Precio < 0)
        {
            return Results.BadRequest(new ErrorResponse
            {
                Exito = false,
                Codigo = "INVALID_PRODUCT_PRICE",
                Mensaje = "El precio del producto no puede ser menor que cero."
            });
        }

        var producto = new Producto
        {
          Id = siguienteId,
          Nombre = request.Nombre,
          Sku = request.Sku,
          Precio = request.Precio
        };

        productos.Add(producto);
        siguienteId++;

        return Results.Created($"/productos/{producto.Id}", producto);

    });

app.Run();

