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

## Estado técnico conocido

El manejo de errores funciona como una primera versión educativa: el flujo trabajado representa la excepción como `400 Bad Request`. Esto no debe considerarse la clasificación definitiva de errores de la API.

## Pendientes relevantes

- Refinar el middleware para distinguir errores de request (`400`) de errores internos (`500`) y de otros casos cuando existan en el proyecto.
- Completar y validar el CRUD de productos: crear, listar, consultar por ID, actualizar y eliminar.
- Confirmar y ampliar las validaciones de nombre/SKU, SKU único y precio no
  negativo (`Price >= 0`); el stock todavía no forma parte del modelo actual.
- Validar la persistencia con SQL Server y EF Core.
- Añadir pruebas proporcionales al riesgo y documentar la ejecución local.
- Revisar el resultado completo y realizar un commit descriptivo cuando se
  confirme que todos los cambios están listos.
