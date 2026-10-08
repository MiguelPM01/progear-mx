# ProGear MX

API REST para gestionar el catálogo de una tienda especializada en herramientas.

ProGear MX es un proyecto de aprendizaje práctico y portafolio. Su primer
vertical es Product Management: modelar productos, consultarlos y validar su
creación mientras se construye una base sólida de ingeniería de software.

## Estado actual

La API trabaja actualmente con productos persistidos en SQL Server mediante
Entity Framework Core. Ya cuenta con:

- consulta de todos los productos: `GET /productos`;
- consulta por identificador: `GET /productos/{id}`;
- consulta del historial de precios: `GET /productos/{id}/precios/historial`;
- consulta del precio vigente: `GET /productos/{id}/precio`;
- consulta de un registro de precio: `GET /productos/{id}/precios/{precioId}`;
- registro y cambio de precio: `POST /productos/{id}/precios`;
- consulta del inventario registrado: `GET /inventario`;
- registro de entradas de mercancía: `POST /inventario/{idProducto}/entradas`;
- creación validada: `POST /productos`;
- validación de nombre y SKU no vacíos;
- validación de SKU duplicado;
- manejo inicial de errores mediante middleware.

La lista local de productos y el identificador manual fueron eliminados. La
API ya consulta e inserta productos en la base de datos; SQL Server genera el
identificador mediante `IDENTITY`. Inventario e historial de precios ya tienen
modelos y mapeos en EF Core. `HistorialPrecio` cuenta con consultas, registro y
cambio transaccional de precio. Inventario ya permite consultar los registros
existentes y registrar entradas de mercancía; las reservas, salidas, liberaciones
y la consulta individual quedan pendientes. Ventas, autenticación, frontend y
otras capacidades quedan fuera del vertical actual.

## Tecnologías

- .NET 10 / ASP.NET Core Minimal API
- C#
- Git y GitHub
- SQL Server y Entity Framework Core

## Estructura

```text
ProGear.api/       API y lógica actual del catálogo
docs/              Cuaderno, glosario y progreso de aprendizaje
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
