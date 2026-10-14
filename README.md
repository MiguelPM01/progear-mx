# ProGear MX

API REST para gestionar el catálogo de una tienda especializada en herramientas.

ProGear MX es un proyecto de aprendizaje práctico y portafolio. Su primer
vertical es Product Management: modelar productos, consultarlos y validar su
creación mientras se construye una base sólida de ingeniería de software.

## Estado actual — 2026-10-14

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
- `POST /inventario/{idProducto}/salidas`;
- `POST /inventario/{idProducto}/liberaciones`.

`Inventario` conserva `Existencia` y `Reservado`; `Disponible` se calcula como
`Existencia - Reservado`. Las entradas requieren una cantidad positiva, las
reservas no pueden superar la disponibilidad y las salidas no pueden superar
lo reservado. Las liberaciones requieren una cantidad positiva que no supere
`Reservado`; una liberación válida reduce solo `Reservado` y conserva la
existencia física. Las solicitudes rechazadas responden con `ErrorResponse`
antes de guardar cambios.

SQL Server genera los identificadores de producto mediante `IDENTITY`. La API
no incluye actualmente rutas de actualización o eliminación de productos. Las
pruebas registradas son manuales. En este checkout,
`docs/requirements/requirements.md` coincide con `docs/progress.md`, por lo que
no hay una matriz de pruebas independiente consolidada.
Ventas y autenticación siguen fuera del vertical actual.

El frontend de `frontend/` usa Vite, React, TypeScript y Tailwind CSS v4.3.3.
Ya tiene una estructura inicial de dashboard: `Header` en la parte superior y,
debajo, `Sidebar` junto al contenido de `AppRoutes`. Las rutas definidas son
`/` (panel principal), `/productos`, `/inventario` y `/precios`, además de una
vista para rutas desconocidas. Las páginas de módulos muestran encabezados de
presentación; todavía no gestionan datos ni se conectan con la API.

`Sidebar` usa `NavLink` con estilos distintos para el enlace activo. La hoja
`index.css` define el tema Tailwind con fondo oscuro de grafito, superficies,
texto claro y acentos azul y rojo; el token actual `--color-primary` es
`#1677E8`. El resumen de una sesión anterior registró `#2589FF`, por lo que
ese valor sigue pendiente de conciliar. Rajdhani e Inter están declaradas, pero
su carga efectiva no se ha comprobado. El control previsto para alternar u
ocultar la Sidebar todavía no tiene comportamiento implementado.

## Tecnologías

- .NET 10 / ASP.NET Core Minimal API
- C#
- Git y GitHub
- SQL Server y Entity Framework Core
- Vite, React, TypeScript y Tailwind CSS v4.3.3 (prototipo frontend)

## Estructura

```text
ProGear.api/       API y lógica actual del catálogo e inventario
frontend/          Prototipo web con Vite, React, TypeScript y Tailwind CSS
docs/              Requerimientos, diseño técnico, pruebas y notas de aprendizaje
docs/frontend/     Diseño y decisiones actuales de la interfaz
ProGear.slnx       Solution de .NET
```

## Ejecutar localmente

```powershell
dotnet run --project ProGear.api
```

Con la aplicación en ejecución, la API queda disponible en la URL que indique
la terminal. Los ejemplos de requests pueden consultarse en
`ProGear.api/ProGear.api.http`.

Para iniciar el prototipo web en otra terminal:

```powershell
Set-Location frontend
npm run dev
```

El navegador mostrará el cascarón inicial del dashboard y las páginas de
presentación de cada ruta. Los flujos de gestión y la conexión con la API aún
no están implementados.

## Enfoque del proyecto

El proyecto prioriza construir un vertical pequeño y comprensible, registrar
el razonamiento y agregar complejidad solo cuando una necesidad concreta la
justifique. El aprendizaje y las decisiones se documentan en
[`docs/notes/learning-notebook.md`](docs/notes/learning-notebook.md), junto con el
[`glosario`](docs/glossary.md), el [`progreso`](docs/progress.md), el
[diseño del frontend](docs/frontend/design.md) y sus
[requerimientos](docs/requirements/frontend-requirements.md).
