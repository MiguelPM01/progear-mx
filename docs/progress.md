# Progreso de ProGear MX

## Alcance actual

ProGear MX es un proyecto de aprendizaje y portafolio cuyo primer vertical es una API REST de gestión de productos. El objetivo inmediato es completar y validar Product Management antes de ampliar el sistema con ventas, autenticación, frontend, nube u otras capacidades.

## Completado o trabajado en la sesión documentada

- Se construyó el esqueleto de `ErrorHandlingMiddleware`.
- Se entendió el papel de `RequestDelegate`, `_next`, `HttpContext` y `InvokeAsync`.
- Se implementó el flujo `try/catch` alrededor de `await _next(context)`.
- Se construyó un `ErrorResponse` con `Exito`, `Codigo` y `Mensaje`.
- Se serializó el error a JSON y se escribió en la response.
- Se corrigió el registro faltante en `Program.cs` mediante `app.UseMiddleware<ErrorHandlingMiddleware>()`.
- Se probó el caso de una request de creación de producto con datos faltantes desde Postman.
- Se creó la bitácora de aprendizaje y el glosario acumulativo.
- Se documentó el flujo de Git: working directory, staging, commit, push,
  `origin/main`, branches, `switch`, `switch -c`, `merge`, `branch -d` y
  upstream mediante `-u`.
- Se mejoró la presentación del repositorio con una descripción, alcance
  actual, tecnologías, estructura y forma de ejecución local.
- Se conectó la API a SQL Server mediante Entity Framework Core.
- Se configuró `ProGearDbContext` y el mapeo de `Producto`, `Inventario` e
  `HistorialPrecio` mediante `OnModelCreating`.
- Se sustituyó la consulta en memoria por `ToListAsync()` para el GET general y
  `FindAsync(id)` para la consulta por identificador.
- Se actualizó la creación de productos para usar `Add` y `SaveChangesAsync()`;
  SQL Server genera el identificador mediante `IDENTITY`.
- Se eliminó la lista local de productos y `siguienteId`.
- Se verificaron casos válidos y de error desde Postman, navegador y consultas
  directas a SQL Server, comprobando que los resultados coinciden.

## Estado actual

**Fase:** Modelado de dominio y base de datos

**Estado:** En progreso

La persistencia básica del catálogo de productos ya está funcionando. El
trabajo pendiente se concentra en las operaciones específicas de inventario,
historial de precios y las pruebas automatizadas correspondientes.

## Sesión documentada — 2026-10-01

- [x] Conectar ASP.NET Core a SQL Server mediante EF Core.
- [x] Configurar `ProGearDbContext` y `OnModelCreating` para las entidades
  actuales.
- [x] Consultar productos desde SQL Server con `ToListAsync()` y
  `FindAsync(id)`.
- [x] Crear productos con `Add` y `SaveChangesAsync()`.
- [x] Confirmar que SQL Server genera los identificadores mediante `IDENTITY`.
- [x] Eliminar la lista de productos en memoria y `siguienteId`.
- [x] Probar creación válida, SKU duplicado, nombre vacío, SKU vacío y request
  incompleto.
- [x] Comparar resultados entre Postman, navegador y consultas directas a SQL
  Server.

### Aprendizajes confirmados

- `async` permite que una operación se ejecute de forma asíncrona y `await`
  espera su resultado.
- `try/catch` permite manejar excepciones.
- `Add` agrega la entidad al seguimiento de EF Core y `SaveChangesAsync()` hace
  persistentes los cambios.
- EF Core traduce expresiones de C# a consultas SQL.
- `DbContext` funciona como contexto y mapa entre los modelos de la aplicación
  y la base de datos.

## Sesión documentada — 2026-09-29

- [x] Expresar el precio vigente como `FechaFin IS NULL`.
- [x] Relacionar la regla con `PRIMARY KEY`, `FOREIGN KEY`, `NOT NULL`, `UNIQUE`, `CHECK` y `DEFAULT`.
- [x] Entender el índice único filtrado para impedir dos precios vigentes del mismo producto.
- [x] Probar el cambio de precio como `UPDATE` + `INSERT` dentro de una transacción.
- [x] Reutilizar `@MomentoCambio` para cerrar el precio anterior y abrir el nuevo con el mismo instante.
- [x] Confirmar el resultado observado: precio anterior cerrado y nuevo precio con `FechaFin = NULL`.

### Estado técnico real

La API ya tiene persistencia básica del catálogo integrada con SQL Server y EF
Core. `Program.cs` usa `ProGearDbContext` para consultar y crear productos; la
lista local y `siguienteId` ya no forman parte del flujo. `Inventario` y
`HistorialPrecio` tienen modelo y mapeo, pero todavía no tienen operaciones
específicas implementadas en la API.

---

## Completado

### Configuración inicial

- [x] Crear solución .NET
- [x] Crear proyecto ASP.NET Core
- [x] Configurar solución
- [x] Ejecutar API
- [x] Crear repositorio Git
- [x] Conectar repositorio con GitHub

### API inicial

- [x] Crear modelo Producto
- [x] Crear DTO para creación de Producto
- [x] Crear GET `/productos`
- [x] Crear GET `/productos/{id}`
- [x] Crear POST `/productos`
- [x] Validar nombre
- [x] Validar SKU
- [x] Validar SKU duplicado
- [x] Validar precio
- [x] Implementar respuestas de error
- [x] Implementar middleware global de errores
- [x] Probar errores 400, 404 y 500
- [x] Probar creación exitosa de producto
- [x] Persistir productos en SQL Server mediante EF Core
- [x] Generar identificadores con SQL Server `IDENTITY`

### Git

- [x] Practicar `git status`
- [x] Practicar `git diff`
- [x] Practicar `git add`
- [x] Practicar `git diff --staged`
- [x] Practicar `git commit`
- [x] Practicar `git push`
- [x] Entender Working Directory
- [x] Entender Staging
- [x] Entender Local Repository
- [x] Entender Remote Repository

### Modelado

- [x] Identificar entidades
- [x] Separar entidades de atributos
- [x] Definir Producto
- [x] Definir Inventario
- [x] Definir HistorialPrecio
- [x] Definir relaciones
- [x] Definir cardinalidades
- [x] Definir reglas de inventario
- [x] Definir historial de precios
- [x] Crear DBML
- [x] Crear ERD en DBDiagram
- [x] Guardar DBML en el repositorio
- [x] Guardar ERD en PNG

---

## Próximo bloque

### Base de datos

- [x] Revisar reglas de integridad del modelo inicial
- [x] Definir la regla de un único precio vigente mediante índice único filtrado
- [x] Probar el cambio de precio con transacción en SQL Server
- [ ] Crear esquema SQL Server
- [ ] Crear tablas
- [ ] Definir restricciones
- [ ] Revisar índices
- [x] Configurar EF Core
- [x] Crear entidades persistentes
- [x] Crear DbContext
- [ ] Ejecutar migraciones
- [x] Sustituir almacenamiento en memoria por SQL Server para productos
- [ ] Implementar operaciones de Inventario e HistorialPrecio en la API

---

## Principio de trabajo

Construir antes que perfeccionar.

Cada bloque seguirá:

`Requerimiento → Diseño → Implementación → Prueba → Revisión → Documentación → Commit`
