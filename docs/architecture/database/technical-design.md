# ProGear MX — Documentación técnica de base de datos

## Propósito y estado

Este documento describe el diseño de `Producto`, `Inventario` e
`HistorialPrecio` y distingue el comportamiento observado en la API del diseño
pendiente. La API ASP.NET Core Minimal API usa SQL Server mediante Entity
Framework Core. Producto tiene consulta general, consulta por identificador y
alta; no hay rutas actuales de actualización o eliminación. `HistorialPrecio`
cuenta con consultas y alta/cambio transaccional. Inventario cuenta con
consulta general y específica, entradas, reservas y salidas. La liberación de
reservas sigue pendiente. El script SQL versionado está en
`docs/architecture/database/sql/pro-gear-mx.sql`; no se encontraron archivos
de migración EF Core en los archivos inspeccionados y el estado del despliegue
de ese script no se confirmó en esta revisión.

## Entidades y relaciones

| Entidad | Propósito | Relación |
|---|---|---|
| `Producto` | Identidad y datos básicos del catálogo | 1 : 0..1 con `Inventario`; 1 : 0..N con `HistorialPrecio` |
| `Inventario` | Existencias y unidades reservadas actuales | Como máximo un registro por producto |
| `HistorialPrecio` | Períodos históricos de precio | Cero, uno o muchos registros por producto |

### Producto

`Id` es la clave primaria e identidad. `Nombre`, `Marca` y `SKU` describen el producto; `SKU` debe ser único.

### Inventario

`IdProducto` es clave foránea hacia `Producto` y es único por producto.
`Existencia` y `Reservado` representan cantidades no negativas, con
`Reservado <= Existencia`. `Disponible = Existencia - Reservado` es un dato
derivado y no se almacena.

La ausencia de fila no equivale a `Existencia = 0`: el primer caso significa que aún no se registró inventario; el segundo, que sí existe un registro sin unidades.

`GET /inventario` combina `Inventario` y `Producto` mediante un `Join` y
proyecta `Disponible = Existencia - Reservado`. `GET
/inventario/{idProducto}` valida primero el producto y después su fila de
inventario; devuelve `INVENTORY_NOT_FOUND` si el producto existe sin fila.

`POST /inventario/{idProducto}/entradas` crea el registro si no existe o
incrementa `Existencia` si ya existe; `Reservado` no cambia durante una
entrada. `POST /inventario/{idProducto}/reservas` requiere cantidad positiva
no mayor que `Disponible`, deja `Existencia` sin cambios e incrementa
`Reservado`. `POST /inventario/{idProducto}/salidas` requiere cantidad positiva
no mayor que `Reservado` y reduce `Existencia` y `Reservado` por igual. Las
validaciones ocurren antes de `SaveChangesAsync()`.

La liberación es diseño pendiente para `POST
/inventario/{idProducto}/liberaciones`: validar cantidad positiva no mayor que
`Reservado`; si se piden 5 y solo hay 3 reservadas, devolver `400 Bad Request`
con `ErrorResponse` (por ejemplo `INSUFFICIENT_RESERVED_STOCK`) y sin mutación.
Una liberación válida reduce solo `Reservado`, conserva `Existencia` y aumenta
`Disponible`. El endpoint aún no está implementado ni probado.

### HistorialPrecio

`IdProducto` es clave foránea hacia `Producto`. `FechaInicio` marca el comienzo de vigencia y `FechaFin` su final. `FechaFin = NULL` significa que el precio sigue vigente.

La entidad se mapea en `OnModelCreating` con `HasKey(h => h.Id)`,
`HasPrecision(10, 2)` para `Precio` y
`HasOne<Producto>().WithMany().HasForeignKey(h => h.IdProducto)`. Esto expresa
la cardinalidad `Producto 1 : 0..N HistorialPrecio`.

## Persistencia actual con EF Core

`ProGearDbContext` expone `DbSet` para las tres entidades y configura nombres
de tabla, claves, relaciones, longitudes requeridas, índices únicos y precisión
decimal en `OnModelCreating`. Las reglas `CHECK` y `DEFAULT` descritas abajo
están en el script SQL; no aparecen configuradas como checks en el mapeo EF
Core inspeccionado.

La API usa actualmente este flujo para productos:

1. `ToListAsync()` consulta todos los productos en SQL Server.
2. `FindAsync(id)` busca un producto por su clave primaria.
3. `Add()` agrega un producto nuevo al seguimiento de EF Core.
4. `SaveChangesAsync()` persiste el cambio.
5. SQL Server genera el `Id` mediante `IDENTITY`.

La lista local de productos y el identificador manual fueron eliminados.

## Operaciones actuales de HistorialPrecio

La API expone tres consultas de solo lectura:

| Endpoint | Consulta EF Core | Respuesta exitosa |
|---|---|---|
| `GET /productos/{id}/precios/historial` | `Where(h => h.IdProducto == id).ToListAsync()` | `200` con colección; `[]` si está vacía |
| `GET /productos/{id}/precio` | `Where(h => h.IdProducto == id && h.FechaFin == null).SingleOrDefaultAsync()` | `200` con un registro o `null` |
| `GET /productos/{id}/precios/{precioId}` | `Where(h => h.IdProducto == id && h.Id == precioId).SingleOrDefaultAsync()` | `200` con el registro perteneciente al producto |

Los tres endpoints devuelven `400` para un identificador de producto no mayor
que cero y `404` si el producto no existe; el endpoint de registro específico
también valida `precioId`. `SingleOrDefaultAsync()` espera cero o un registro:
devuelve `null` sin coincidencias y señala un estado inconsistente si encuentra
más de uno. La consulta de historial materializa una colección, por lo que
cero resultados se representan como `[]`.

El endpoint `POST /productos/{id}/precios` valida producto, precio positivo y
no duplicación del precio vigente. Si existe un precio vigente, le asigna
`FechaFin`; después agrega el nuevo registro con `FechaInicio` en el mismo
instante y `FechaFin = NULL`.

## Operaciones actuales de Inventario

La API expone dos consultas y tres operaciones de escritura:

| Endpoint | Flujo EF Core | Respuesta exitosa |
|---|---|---|
| `GET /inventario` | `Inventarios.Join(Productos, ...)` y `ToListAsync()` | `200` con el resumen de cada inventario registrado |
| `GET /inventario/{idProducto}` | `FindAsync`, `SingleOrDefaultAsync` y proyección a `InventarioResponse` | `200`; `400` por ID inválido; `404` por producto o inventario ausente |
| `POST /inventario/{idProducto}/entradas` | `FindAsync`, `SingleOrDefaultAsync`, `Add` cuando corresponde y `SaveChangesAsync()` | `201` con `InventarioResponse` |
| `POST /inventario/{idProducto}/reservas` | Valida cantidad contra `Disponible`, incrementa `Reservado` y guarda | `200` con `InventarioResponse` |
| `POST /inventario/{idProducto}/salidas` | Valida cantidad contra `Reservado`, reduce existencia y reserva y guarda | `200` con `InventarioResponse` |

La ausencia de inventario es válida para un producto. Por eso la primera
entrada crea el registro y no devuelve `INVENTORY_NOT_FOUND`.

La matriz de pruebas manuales se conserva agrupada por módulo en
`docs/requirements/requirements.md`. Incluye las consultas `PEG-001` a
`PEG-004` de Inventario, entradas `PEI-001` a `PEI-008`, reservas `PEP-001` a
`PEP-009` y salidas `PEP-001` a `PEP-005`. Los prefijos se repiten en módulos
diferentes. Las pruebas de salidas `PEP-003` a `PEP-005` incluyen una consulta
posterior que confirmó que los rechazos no cambiaron `Existencia` ni
`Reservado`.

## Constraints e índice único filtrado

| Regla | Mecanismo |
|---|---|
| Identificar filas | `PRIMARY KEY` |
| Evitar registros huérfanos | `FOREIGN KEY` |
| Evitar SKU duplicado | `UNIQUE` |
| Proteger condiciones de dominio | `CHECK` |
| Completar valores automáticos | `DEFAULT` |
| Permitir solo un precio vigente por producto | Índice único filtrado por `IdProducto` donde `FechaFin IS NULL` |

El script `docs/architecture/database/sql/pro-gear-mx.sql` contiene las
restricciones de `CHECK` para el formato de SKU, las cantidades de inventario
y los valores/fechas de precio; incluye `DEFAULT` para `Existencia`,
`Reservado` y `FechaInicio`, además de `UX_HistorialPrecio_UnicoVigente`, un
índice único filtrado por `IdProducto` donde `FechaFin IS NULL`. El estado de
ejecución de ese script y la correspondencia exacta con una base desplegada no
se verificaron en esta revisión.

## Cambio de precio mediante transacción

```sql
BEGIN TRANSACTION;

DECLARE @MomentoCambio DATETIME2 = SYSDATETIME();

UPDATE HistorialPrecio
SET FechaFin = @MomentoCambio
WHERE IdProducto = 1
  AND FechaFin IS NULL;

INSERT INTO HistorialPrecio (
    IdProducto, Precio, FechaInicio, FechaFin
)
VALUES (
    1, 1600.00, @MomentoCambio, NULL
);

COMMIT TRANSACTION;
```

En la implementación actual, el instante se captura una sola vez en
`momentoCambio`. `BeginTransactionAsync()` inicia la operación, `CommitAsync()`
confirma y `RollbackAsync()` revierte si una parte falla.

## Trazabilidad

- **Requisito:** un producto no debe tener más de un precio vigente.
- **Modelo:** el precio vigente tiene `FechaFin = NULL`.
- **Protección:** índice único filtrado por `IdProducto` con `FechaFin IS NULL`.
- **Operación:** cerrar con `UPDATE` y crear con `INSERT` dentro de una transacción.
- **Resultado observado:** el precio anterior quedó cerrado y el nuevo quedó vigente.
- **Prueba controlada:** una excepción provocada antes del commit devolvió `500`
  mediante el middleware; el precio anterior permaneció vigente y el nuevo no
  quedó confirmado.

## Pendientes explícitos

- Implementar y probar la liberación de reservas.
- Resolver `PEI-008` y reconciliar el código transcrito en las pruebas de salida
  `PEP-004` y `PEP-005` con `INVALID_INVENTORY_QUANTITY` del handler actual.
- Reconciliar el código `PRODUCT_NOT_fOUND` de `GET /productos/{id}` con el
  contrato `PRODUCT_NOT_FOUND` documentado.
- Reconciliar `CK_HistorialPrecio_Precio_NoNegativo` (permite cero) con la
  validación actual de la API (rechaza precios menores o iguales a cero).
- Confirmar el despliegue del script SQL y resolver la estrategia/estado de
  migraciones EF Core.
- Crear pruebas automatizadas; las pruebas PEG/PEP documentadas hasta ahora son manuales.
