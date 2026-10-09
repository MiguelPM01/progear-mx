# ProGear MX

API REST para gestionar el catálogo de una tienda especializada en herramientas.

ProGear MX es un proyecto de aprendizaje práctico y portafolio. Su primer
vertical es Product Management: modelar productos, consultarlos y validar su
creación mientras se construye una base sólida de ingeniería de software.

## Estado actual — 2026-10-09

La API ASP.NET Core Minimal API usa Entity Framework Core y SQL Server. El
catálogo permite consultar productos y registrar productos nuevos; el precio
se conserva en `HistorialPrecio`. Las rutas actualmente implementadas son:

- `GET /productos` y `GET /productos/{id}`;
- `POST /productos`;
- `GET /productos/{id}/precios/historial`;
- `GET /productos/{id}/precio`;
- `GET /productos/{id}/precios/{precioId}`;
- `POST /productos/{id}/precios`, con cierre del precio anterior y alta del
  nuevo dentro de una transacción;
- `GET /inventario` y `GET /inventario/{idProducto}`;
- `POST /inventario/{idProducto}/entradas`;
- `POST /inventario/{idProducto}/reservas`;
- `POST /inventario/{idProducto}/salidas`.

`Inventario` conserva `Existencia` y `Reservado`; `Disponible` se calcula como
`Existencia - Reservado`. Las entradas requieren una cantidad positiva, las
reservas no pueden superar la disponibilidad y las salidas no pueden superar
lo reservado. Las solicitudes rechazadas responden con `ErrorResponse` antes
de guardar cambios. El endpoint de liberación aún está pendiente: al
implementarlo, validará una cantidad positiva que no supere `Reservado` y solo
reducirá `Reservado`, sin modificar la existencia física.

SQL Server genera los identificadores de producto mediante `IDENTITY`. La API
no incluye actualmente rutas de actualización o eliminación de productos. Las
pruebas registradas son manuales; su matriz por módulo se mantiene en
[`docs/requirements/requirements.md`](docs/requirements/requirements.md).
Ventas, autenticación y frontend siguen fuera del vertical actual.

## Tecnologías

- .NET 10 / ASP.NET Core Minimal API
- C#
- Git y GitHub
- SQL Server y Entity Framework Core

## Estructura

```text
ProGear.api/       API y lógica actual del catálogo e inventario
docs/              Requerimientos, diseño técnico, pruebas y notas de aprendizaje
ProGear.slnx       Solution de .NET
```

## Ejecutar localmente

```powershell
dotnet run --project ProGear.api
```

Con la aplicación en ejecución, la API queda disponible en la URL que indique
la terminal. Los ejemplos de requests pueden consultarse en
`ProGear.api/ProGear.api.http`.

## Enfoque del proyecto

El proyecto prioriza construir un vertical pequeño y comprensible, registrar
el razonamiento y agregar complejidad solo cuando una necesidad concreta la
justifique. El aprendizaje y las decisiones se documentan en
[`docs/notes/learning-notebook.md`](docs/notes/learning-notebook.md), junto con el
[`glosario`](docs/glossary.md) y el [`progreso`](docs/progress.md).
