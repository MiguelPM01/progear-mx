# Progreso de ProGear MX

## Alcance actual

ProGear MX es un proyecto de aprendizaje y portafolio. El vertical actual
cubre el catálogo de productos, el historial de precios y las operaciones
básicas de inventario. El frontend ya tiene un prototipo inicial en trabajo;
el dashboard final y la conexión con la API todavía no están hechos. Ventas,
autenticación y nube quedan fuera del alcance inmediato.

## Completado o trabajado

- API ASP.NET Core Minimal API conectada a SQL Server mediante Entity Framework
  Core; `ProGearDbContext` mapea `Producto`, `Inventario` e `HistorialPrecio`.
- Consulta y alta de productos persistidas con EF Core; SQL Server genera el
  identificador mediante `IDENTITY`. No hay rutas actuales de actualización o
  eliminación de productos.
- Consultas de historial, precio vigente y registro individual de precio; alta
  y cambio transaccional de precio con `CommitAsync()` y `RollbackAsync()`.
- Inventario: `GET /inventario`, `GET /inventario/{idProducto}`, entradas,
  reservas, salidas y liberaciones. `Disponible` se calcula como
  `Existencia - Reservado`.
- Reglas implementadas: cantidad de operación positiva; una reserva no supera
  `Disponible`; una salida no supera `Reservado`; una salida válida reduce
  `Existencia` y `Reservado` por la misma cantidad; una liberación no supera
  `Reservado` y reduce únicamente ese valor.
- Se conservan `ErrorHandlingMiddleware`, `ErrorResponse` y las validaciones
  de catálogo documentadas desde sesiones anteriores.
- Frontend inicial con Vite, React, TypeScript y Tailwind CSS v4.3.3. El nodo
  `#root` carga `main.tsx`, que monta `App.tsx`; la aplicación actual sirve
  para probar la paleta y las utilidades CSS, no es todavía el dashboard.
- Dirección de marca industrial/de carreras: base oscura de grafito y plata,
  acentos discretos azul y rojo, Rajdhani en títulos e Inter en texto general.
  Las fuentes se declararon; su carga efectiva queda por comprobar.
- Se verificaron casos de producto mediante Postman, navegador y consultas
  directas a SQL Server. El detalle disponible de todas las pruebas manuales
  está agrupado por módulo en
  [`docs/requirements/requirements.md`](requirements/requirements.md).

## Estado actual

**Fase:** Prototipo inicial de frontend

**Estado:** En progreso — catálogo e `HistorialPrecio` están documentados como
completados. `Inventario` ya permite consultar, ingresar, reservar, dar salida
y liberar unidades reservadas. Se inició el prototipo web; su dashboard e
integración con la API siguen pendientes.

Las pruebas documentadas son manuales. La sesión del 2026-10-12 registra
`PEL-001` a `PEL-008` para la operación de liberaciones. Los identificadores
`PEG`, `PEP` y `PEL` se agrupan por módulo y operación; no deben leerse como
secuencias globales.

## Sesión documentada — 2026-10-12

- [x] Iniciar el frontend en `frontend/` con Vite, React, TypeScript y
  Tailwind CSS v4.3.3.
- [x] Conectar el nodo `#root` del HTML con el punto de entrada `main.tsx`,
  que monta el componente `App`.
- [x] Comprobar que HMR actualiza la página durante el desarrollo.
- [x] Corregir el orden de importaciones CSS: fuentes de Google antes de
  `@import "tailwindcss"`; verificar en el navegador que las utilidades de
  color se aplican.
- [x] Definir tokens `@theme` y dirección visual inicial para ProGear MX.
- [x] Implementar `POST /inventario/{idProducto}/liberaciones` con cantidad
  positiva, límite `Cantidad <= Reservado` y actualización exclusiva de
  `Reservado`.
- [x] Registrar la ejecución manual de `PEL-001` a `PEL-008`.
- [ ] Incorporar los escenarios y resultados individuales de `PEL-001` a
  `PEL-008` a la matriz cuando se recupere ese detalle.

### Aprendizaje y estado visual

`index.html` contiene el nodo que React necesita para insertar la interfaz.
`main.tsx` es el punto de entrada y entrega la composición al componente
`App.tsx`; dividir la interfaz en componentes permite construir piezas
reutilizables. HMR refleja los cambios en la página sin detener el servidor.

Tailwind CSS v4 permite declarar colores y fuentes como tokens de `@theme`.
Las reglas CSS externas deben importarse en el orden correcto para que Tailwind
las procese; las utilidades de color ya se observaron en el navegador. Aunque
Rajdhani e Inter están declaradas, la carga efectiva de ambas fuentes sigue
sin verificación separada.

El resumen de la sesión indica `--color-primary: #2589FF`, pero el archivo
`frontend/src/index.css` revisado en el checkout contiene `#1677E8`. Queda por
decidir cuál valor representa el token vigente antes de construir más UI.

### Siguiente bloque

- Verificar que Rajdhani e Inter carguen en el navegador.
- Definir estilos globales.
- Construir `Sidebar` y `Header` como primeros componentes del dashboard.
- Conectar el frontend con la API en una sesión posterior.
- Completar el detalle de la matriz manual `PEL-001` a `PEL-008`.

### Cierre

La base React y la prueba visual de Tailwind ya se muestran en el navegador;
el diseño del dashboard y la conexión con el backend aún son trabajo futuro.
Las liberaciones de inventario están implementadas y la sesión registra ocho
pruebas manuales, aunque falta recuperar el detalle individual de cada una.

## Sesión documentada — 2026-10-09

- [x] Implementar y probar `GET /inventario/{idProducto}`.
- [x] Implementar `POST /inventario/{idProducto}/reservas`: mantener
  `Existencia`, aumentar `Reservado` y rechazar cantidades mayores que
  `Disponible`.
- [x] Implementar `POST /inventario/{idProducto}/salidas`: reducir
  `Existencia` y `Reservado` solo si la cantidad no supera `Reservado`.
- [x] Registrar pruebas de consulta `PEG-001` a `PEG-004` del módulo
  `Inventario`.
- [x] Registrar pruebas de reservas `PEP-001` a `PEP-009` y de salidas
  `PEP-001` a `PEP-005`, separadas por operación.
- [x] Confirmar con una consulta GET que las salidas rechazadas por cantidad
  insuficiente, cero o negativa no cambiaron las existencias ni las reservas.
- [ ] Implementar y probar `POST /inventario/{idProducto}/liberaciones`.
- [ ] Resolver `PEI-008`, que sigue pendiente como prueba de entrada con una
  reserva existente.

### Aprendizaje del flujo

Una reserva reduce `Disponible` sin tocar la existencia física. Una salida
confirmada reduce tanto la existencia como la reserva. Una liberación pendiente
debe reducir únicamente `Reservado`, después de validar una cantidad positiva
que no supere lo reservado; así se recupera disponibilidad sin alterar el
stock físico. Los detalles y límites de cada prueba están en la matriz de
requerimientos.

### Cierre de la sesión

Las consultas, reservas y salidas de inventario quedan implementadas y con
pruebas manuales registradas. La liberación continúa pendiente; su regla está
documentada como diseño acordado, no como comportamiento ya disponible.

## Sesión documentada — 2026-10-08

- [x] Documentar el modelo y las reglas de `Inventario`.
- [x] Mantener la semántica `Producto 1 : 0..1 Inventario`.
- [x] Crear `InventarioResponse` como DTO de salida.
- [x] Crear `CantidadRequest` como DTO reutilizable de entrada.
- [x] Implementar `GET /inventario` mediante `Join`.
- [x] Implementar `POST /inventario/{idProducto}/entradas`.
- [x] Validar identificador, producto inexistente y cantidad no positiva con
  `ErrorResponse`.
- [x] Registrar las pruebas manuales `PEI-001` a `PEI-007`.
- [ ] Ejecutar `PEI-008`; quedó pendiente al cierre del 2026-10-08 y sigue sin
  resultado registrado al 2026-10-09.

### Cierre de la sesión

La primera entrega de `Inventario` queda documentada. El siguiente trabajo es
implementar y probar reservas, salidas, liberaciones y la consulta individual,
sin atribuir esos comportamientos a esta sesión.

## Sesión documentada — 2026-10-06

- [x] Completar `GET /productos/{id}/precios/historial`.
- [x] Completar `GET /productos/{id}/precio`.
- [x] Completar `GET /productos/{id}/precios/{precioId}`.
- [x] Completar `POST /productos/{id}/precios` para primer precio y cambio.
- [x] Validar identificadores, precio menor o igual que cero y precio duplicado.
- [x] Confirmar cierre del precio anterior y creación del nuevo precio vigente.
- [x] Confirmar transacción con `Commit`.
- [x] Ejecutar prueba controlada de `Rollback` y comprobar que `$2,100` permaneció vigente.
- [x] Retirar la condición artificial y confirmar una prueba normal posterior.
- [x] Documentar pruebas `PEG-001` a `PEG-009` y `PEP-010` a `PEP-015`.

### Cierre de HistorialPrecio

`HistorialPrecio` queda cerrado conforme a las reglas definidas antes de su
desarrollo. El siguiente bloque de trabajo es `Inventario`.

### Pruebas registradas

La matriz completa y los resultados están en `docs/requirements/requirements.md`.
Se conserva la convención `PEG = Prueba Endpoint GET` y `PEP = Prueba Endpoint POST`.

## Sesión documentada — 2026-10-02

- [x] Completar `GET /productos/{id}/precios/historial`.
- [x] Completar `GET /productos/{id}/precio`.
- [x] Validar identificadores inválidos con `400`.
- [x] Validar productos inexistentes con `404`.
- [x] Confirmar `200` con `[]` para un historial vacío.
- [x] Confirmar `200` con `null` cuando el producto existe pero no tiene precio
  vigente.
- [x] Confirmar `200` con los datos del historial o del precio vigente cuando
  existen.
- [x] Revisar el mapeo EF Core de `HistorialPrecio` y la traducción conceptual
  de las consultas LINQ a SQL.
- [x] Resolver mediante debugging basado en evidencia que el `1` visible en
  Postman era el número de línea del editor y no el contenido de la respuesta.

### Estado técnico al cierre

Los endpoints de consulta de historial y precio vigente están documentados con
el contrato `400/404/200` observado. En la sesión posterior del 2026-10-06 se
completó el cambio de precio y se cerró `HistorialPrecio`.

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

### Frontend

- [ ] Verificar que Rajdhani e Inter carguen correctamente.
- [ ] Definir estilos globales.
- [ ] Construir los componentes `Sidebar` y `Header`.
- [ ] Planear y conectar el frontend con la API.
- [ ] Conciliar `--color-primary` entre el valor consignado en la sesión
  (`#2589FF`) y el valor presente en el checkout (`#1677E8`).

### Inventario

- [x] Implementar y probar manualmente `POST /inventario/{idProducto}/liberaciones`.
- [ ] Incorporar el detalle individual de los casos `PEL-001` a `PEL-008`.
- [ ] Resolver `PEI-008` y registrar su resultado.
- [ ] Repetir `PEP-004` y `PEP-005` de salidas para confirmar el código de error exacto.

### Alineación técnica

- [ ] Alinear el código de error del producto inexistente en
  `GET /productos/{id}` con `PRODUCT_NOT_FOUND` y verificar la respuesta.
- [ ] Reconciliar la restricción SQL que permite precio cero con la validación
  actual de la API, que rechaza precios menores o iguales a cero.
- [ ] Reconciliar el script SQL versionado con el mapeo EF Core y confirmar el
  estado de despliegue/migraciones.

---

## Principio de trabajo

Construir antes que perfeccionar.

Cada bloque seguirá:

`Requerimiento → Diseño → Implementación → Prueba → Revisión → Documentación → Commit`
