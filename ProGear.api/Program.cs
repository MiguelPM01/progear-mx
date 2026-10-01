using Microsoft.EntityFrameworkCore;
using ProGear.Api.Data;

using ProGear.Api;
using ProGear.Api.Models;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ProGearDbContext>(options =>
options.UseSqlServer(
    builder.Configuration.GetConnectionString("ProGear")
));

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

/*---Configuración de las rutas GET para solicitar información de productos en la base de datos a través de la API.---*/
    app.MapGet("/productos", async (ProGearDbContext context) =>
    {
       var productos = await context.Productos.ToListAsync();

       return Results.Ok(productos);

    });

    app.MapGet("/productos/{id}",  async (int id, ProGearDbContext context) =>
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
        var productos = await context.Productos.FindAsync(id);

        if(productos != null)
        {
            return Results.Ok(productos);
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

/*---Configuración de la ruta POST para crear nuevos productos en la base de datos a través de la API.---*/
    app.MapPost("/productos", async (CrearProductoRequest request, ProGearDbContext context) =>
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

        if (await context.Productos.AnyAsync(p => p.Sku == request.Sku))
        {
            return Results.Conflict(new ErrorResponse
            {
               Exito = false,
               Codigo = "DUPLICATED_SKU",
               Mensaje = "El SKU del producto ya está registrado." 
            });
        }

       

        var producto = new Producto
        {
          Nombre = request.Nombre,
          Marca = request.Marca,
          Sku = request.Sku,
        };

        context.Productos.Add(producto);

        await context.SaveChangesAsync();

        return Results.Created($"/productos/{producto.Id}", producto);

    });

app.Run();

 /*if (request.Precio < 0)
        {
            return Results.BadRequest(new ErrorResponse
            {
                Exito = false,
                Codigo = "INVALID_PRODUCT_PRICE",
                Mensaje = "El precio del producto no puede ser menor que cero."
            });
        }*/