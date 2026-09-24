# ProGear MX

API REST para gestionar el catálogo de una tienda especializada en herramientas.

ProGear MX es un proyecto de aprendizaje práctico y portafolio. Su primer
vertical es Product Management: modelar productos, consultarlos y validar su
creación mientras se construye una base sólida de ingeniería de software.

## Estado actual

La API trabaja actualmente con productos en memoria. Ya cuenta con:

- consulta de todos los productos: `GET /productos`;
- consulta por identificador: `GET /productos/{id}`;
- creación validada: `POST /productos`;
- validación de nombre y SKU no vacíos;
- validación de SKU duplicado y precio no negativo;
- manejo inicial de errores mediante middleware.

La persistencia con SQL Server y Entity Framework Core es el siguiente paso
del proyecto. Ventas, autenticación, frontend y otras capacidades quedan
fuera del vertical actual.

## Tecnologías

- .NET 10 / ASP.NET Core Minimal API
- C#
- Git y GitHub
- SQL Server y Entity Framework Core (siguiente etapa)

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
[`docs/learning-notebook.md`](docs/learning-notebook.md), junto con el
[`glosario`](docs/glossary.md) y el [`progreso`](docs/progress.md).
