using Microsoft.EntityFrameworkCore;
using ProGear.Api.Data;
using ProGear.Api.Models;
using ProGear.Api.DTOs;
using ProGear.Api.Middleware;
using ProGear.Api.Responses;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ProGearDbContext>(options =>
options.UseSqlServer(
    builder.Configuration.GetConnectionString("ProGear")
));

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();
/*--------------------------------------------------------------------------------------------------------------------*/
/*---Configuración de las rutas GET para solicitar información de productos en la base de datos a través de la API.---*/
/*-------------------------------------------------------------------------------------------------------------------*/

/*---Configuración de la ruta GET para obtener todos los productos de la base de datos a través de la API.---*/
    app.MapGet("/productos", async (ProGearDbContext context) =>
    {
       var productos = await context.Productos.ToListAsync();

       return Results.Ok(productos);

    });

/*---Configuración de la ruta GET para obtener un producto específico de la base de datos a través de la API.---*/
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

/*---Configuración de la ruta GET para obtener el historial de precios de un producto específico de la base de datos a través de la API.---*/
    app.MapGet("/productos/{id}/precios/historial", async (int id, ProGearDbContext context)=>
    {
        if (id <= 0)
        {
            return Results.BadRequest( new ErrorResponse
            {
               Exito = false,
               Codigo = "INVALID_PRODUCT_ID",
               Mensaje = "El identificador del producto debe ser mayor a cero." 
            });
        }

        var producto = await context.Productos.FindAsync(id);

        if (producto == null)
        {
             return Results.NotFound( new ErrorResponse
            {
               Exito = false,
               Codigo = "PRODUCT_NOT_FOUND",
               Mensaje = "No encontramos un producto con el identificador proporcionado." 
            });  
        }

        var historial = await context.HistorialPrecios
            .Where( h => h.IdProducto == id)
            .ToListAsync();

            return Results.Ok(historial);

    });

/*---Configuración de la ruta GET para obtener el precio vigente de un producto específico de la base de datos a través de la API.---*/
    app.MapGet("/productos/{id}/precio", async (int id, ProGearDbContext context) =>
    {
       if (id <= 0)
        {
            return Results.BadRequest(new ErrorResponse
            {
               Exito = false,
               Codigo= "INVALID_PRODUCT_ID",
               Mensaje = "El identificador del producto debe ser mayor a cero." 
            });
        } 

        var producto = await context.Productos.FindAsync(id);

        if (producto == null)
        {
            return Results.NotFound(new ErrorResponse
            {
               Exito = false,
               Codigo = "PRODUCT_NOT_FOUND",
               Mensaje = "No encontramos un producto con el identificador proporcionado." 
            });
        }

        var precioVigente = await context.HistorialPrecios
            .Where(h => h.IdProducto == id && h.FechaFin == null)
            .SingleOrDefaultAsync();

            return Results.Ok(precioVigente);
    });

    /*---Configuración de la ruta GET para obtener el registro de precios históricos de un producto específico.---*/

    app.MapGet("/productos/{id}/precios/{precioId}", async (int id, int precioId, ProGearDbContext context) =>
    {
        if (id <= 0)
        {
            return Results.BadRequest(new ErrorResponse
            {
                Exito = false,
                Codigo = "INVALID_PRODUCT_ID",
                Mensaje = "El identificador del producto debe ser mayor a cero."
            });
        }

        var producto = await context.Productos.FindAsync(id);

        if (producto == null)
        {
            return Results.NotFound(new ErrorResponse
            {
                Exito = false,
                Codigo = "PRODUCT_NOT_FOUND",
                Mensaje = "No encontramos un producto con el identificador proporcionado."
            });
        }

        if (precioId <= 0)
        {
            return Results.BadRequest(new ErrorResponse
            {
                Exito = false,
                Codigo = "INVALID_PRICE_ID",
                Mensaje = "El identificador del precio debe ser mayor a cero."
            });
        }

        var precioHistorico = await context.HistorialPrecios
            .Where(h => h.IdProducto == id && h.Id == precioId)
            .SingleOrDefaultAsync();

        if (precioHistorico == null)
        {
            return Results.NotFound(new ErrorResponse
            {
                Exito = false,
                Codigo = "PRICE_HISTORY_NOT_FOUND",
                Mensaje = "No encontramos el registro específico del historial de precios de este producto."
            });
        }

        return Results.Ok(precioHistorico);

    });
/*---Configuración de la ruta GET para obtener el inventario general de productos.---*/
    app.MapGet("/inventario", async (ProGearDbContext context) =>
    {
       var inventario = await context.Inventarios
            .Join(
                context.Productos,
                inventario => inventario.IdProducto,
                producto => producto.Id,
                (inventario, producto) => new
                {
                    IdProducto = producto.Id,
                    Nombre = producto.Nombre,
                    Marca = producto.Marca,
                    Sku = producto.Sku,
                    Existencia = inventario.Existencia,
                    Reservado = inventario.Reservado,
                    Disponible = inventario.Existencia - inventario.Reservado
                })
                .ToListAsync();
        
        return Results.Ok(inventario);

    });
 
/*------------------------------------------------------------------------------------------------------------------*/
/*--- Configuración de las rutas POST para mandar iformación de productos a la base de datos a través de la API.---*/
/*----------------------------------------------------------------------------------------------------------------*/

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


/*--- Configuración de la ruta POST para agregar precios a productos existentes. ---*/
app.MapPost("/productos/{id}/precios", async (int id, PrecioProductoRequest request, ProGearDbContext context) =>
{
    if (id <= 0)
    {
        return Results.BadRequest(new ErrorResponse
        {
            Exito = false,
            Codigo = "INVALID_PRODUCT_ID",
            Mensaje = "El identificador del producto debe ser mayor a cero."
        });
    }

    var producto = await context.Productos.FindAsync(id);

    if (producto == null)
    {
        return Results.NotFound(new ErrorResponse
        {
            Exito = false,
            Codigo = "PRODUCT_NOT_FOUND",
            Mensaje = "No encontramos un producto con el identificador proporcionado."
        });
    }

    if (request.Precio <= 0)
    {
        return Results.BadRequest(new ErrorResponse
        {
           Exito = false,
           Codigo = "INVALID_PRODUCT_PRICE",
            Mensaje = "El precio del producto debe ser mayor a cero." 
        });
    }

     var precioVigente = await context.HistorialPrecios
        .Where(h => h.IdProducto == id && h.FechaFin == null)
        .SingleOrDefaultAsync();

    if (precioVigente != null && request.Precio == precioVigente.Precio)
    {
        return Results.BadRequest(new ErrorResponse
        {
            Exito = false,
            Codigo = "DUPLICATED_PRICE",
            Mensaje = "El precio registrado no debe ser igual al precio vigente del producto."
        });
    }

    var momentoCambio = DateTime.Now;

    await using var transaction = await context.Database.BeginTransactionAsync();

    try
    {
        if (precioVigente != null)
        {
            precioVigente.FechaFin = momentoCambio;
        }

        var nuevoPrecio = new HistorialPrecio
        {
            IdProducto = id,
            Precio = request.Precio,
            FechaInicio = momentoCambio,
            FechaFin = null
        };

        context.HistorialPrecios.Add(nuevoPrecio);

        await context.SaveChangesAsync();

        await transaction.CommitAsync();

        return Results.Created($"/productos/{id}/precios/{nuevoPrecio.Id}", nuevoPrecio);
    }
    catch
    {
        await transaction.RollbackAsync();
        throw;

    }
});

/*---Configuración de la ruta POST para registrar existencias en el sistema---*/

app.MapPost("/inventario/{idProducto}/entradas", async (int idProducto, CantidadRequest request, ProGearDbContext context) =>
{
   if (idProducto <= 0 )
    {
        return Results.BadRequest(new ErrorResponse
        {
            Exito = false,
            Codigo ="INVALID_PRODUCT_ID",
        Mensaje = "El identificador del producto debe ser mayor a cero."
        });
    }

    var producto = await context.Productos.FindAsync(idProducto);

    if (producto == null)
    {
        return Results.NotFound( new ErrorResponse
        {
           Exito = false,
           Codigo = "PRODUCT_NOT_FOUND",
           Mensaje = "No encontramos un producto con el identificador proporcionado."
        });
    }

    if (request.Cantidad <= 0)
    {
        return Results.BadRequest(new ErrorResponse
        {
            Exito = false,
            Codigo = "INVALID_INVENTORY_QUANTITY",
            Mensaje = "La cantidad debe ser mayor a cero."
        });
    }

    var inventario = await context.Inventarios
        .SingleOrDefaultAsync(i => i.IdProducto == idProducto);

    if (inventario == null)
    {
        inventario = new Inventario
        {
            IdProducto = idProducto,
            Existencia = request.Cantidad,
            Reservado = 0
        };

        context.Inventarios.Add(inventario);

    }
    else
    {
        inventario.Existencia += request.Cantidad;
    }

    await context.SaveChangesAsync();

    var response = new InventarioResponse
    {
        IdProducto = producto.Id,
        Nombre = producto.Nombre,
        Marca = producto.Marca,
        Sku = producto.Sku,
        Existencia = inventario.Existencia,
        Reservado = inventario.Reservado,
        Disponible = inventario.Existencia - inventario.Reservado
    };

    return Results.Created($"/inventario/{idProducto}", response);
});

app.Run();