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

## Estado actual

**Fase:** Modelado de dominio y base de datos

**Estado:** En progreso

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

- [ ] Revisar reglas de integridad
- [ ] Crear esquema SQL Server
- [ ] Crear tablas
- [ ] Definir restricciones
- [ ] Revisar índices
- [ ] Configurar EF Core
- [ ] Crear entidades persistentes
- [ ] Crear DbContext
- [ ] Ejecutar migraciones
- [ ] Sustituir almacenamiento en memoria por SQL Server

---

## Principio de trabajo

Construir antes que perfeccionar.

Cada bloque seguirá:

`Requerimiento → Diseño → Implementación → Prueba → Revisión → Documentación → Commit`