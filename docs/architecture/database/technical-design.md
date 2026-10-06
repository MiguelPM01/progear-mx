# ProGear MX — Documentación técnica de base de datos

## Propósito y estado

Este documento describe la documentación técnica de `Producto`, `Inventario` e
`HistorialPrecio`. El modelo conceptual está en `docs/database/erd/progear-mx.dbml`.
La API ya usa SQL Server mediante Entity Framework Core para las operaciones
básicas de productos y para consultar y registrar precios. Todavía no existe
una migración documentada aquí; las operaciones específicas de inventario
siguen pendientes.

## Entidades y relaciones

| Entidad | Propósito | Relación |
|---|---|---|
| `Producto` | Identidad y datos básicos del catálogo | 1 : 0..1 con `Inventario`; 1 : 0..N con `HistorialPrecio` |
| `Inventario` | Existencias y unidades reservadas actuales | Como máximo un registro por producto |
| `HistorialPrecio` | Períodos históricos de precio | Cero, uno o muchos registros por producto |

### Producto

`Id` es la clave primaria e identidad. `Nombre`, `Marca` y `SKU` describen el producto; `SKU` debe ser único.

### Inventario

`IdProducto` es clave foránea hacia `Producto` y es único por producto. `Existencia` y `Reservado` representan cantidades no negativas. `Disponible = Existencia - Reservado` es un dato derivado y no se almacena.

La ausencia de fila no equivale a `Existencia = 0`: el primer caso significa que aún no se registró inventario; el segundo, que sí existe un registro sin unidades.

### HistorialPrecio

`IdProducto` es clave foránea hacia `Producto`. `FechaInicio` marca el comienzo de vigencia y `FechaFin` su final. `FechaFin = NULL` significa que el precio sigue vigente.

La entidad se mapea en `OnModelCreating` con `HasKey(h => h.Id)`,
`HasPrecision(10, 2)` para `Precio` y
`HasOne<Producto>().WithMany().HasForeignKey(h => h.IdProducto)`. Esto expresa
la cardinalidad `Producto 1 : 0..N HistorialPrecio`.

## Persistencia actual con EF Core

`ProGearDbContext` expone `DbSet` para las tres entidades y configura sus
tablas, claves, relaciones, longitudes, obligatoriedad, unicidad y precisión
decimal en `OnModelCreating`.

La API usa actualmente este flujo para productos:

1. `ToListAsync()` consulta todos los productos en SQL Server.
2. `FindAsync(id)` busca un producto por su clave primaria.
3. `Add()` agrega un producto nuevo al seguimiento de EF Core.
4. `SaveChangesAsync()` persiste el cambio.
5. SQL Server genera el `Id` mediante `IDENTITY`.

La lista local de productos y el identificador manual fueron eliminados.

## Operaciones actuales de HistorialPrecio

La API expone dos consultas de solo lectura:

| Endpoint | Consulta EF Core | Respuesta exitosa |
|---|---|---|
| `GET /productos/{id}/precios/historial` | `Where(h => h.IdProducto == id).ToListAsync()` | `200` con colección; `[]` si está vacía |
| `GET /productos/{id}/precio` | `Where(h => h.IdProducto == id && h.FechaFin == null).SingleOrDefaultAsync()` | `200` con un registro o `null` |
| `GET /productos/{id}/precios/{precioId}` | `Where(h => h.IdProducto == id && h.Id == precioId).SingleOrDefaultAsync()` | `200` con el registro perteneciente al producto |

Ambos endpoints devuelven `400` para un identificador no mayor que cero y
`404` si el producto no existe. `SingleOrDefaultAsync()` espera cero o un
registro: devuelve `null` sin coincidencias y señala un estado inconsistente
si encuentra más de uno. La consulta de historial materializa una colección,
por lo que cero resultados se representan como `[]`.

El endpoint `POST /productos/{id}/precios` valida producto, precio positivo y
no duplicación del precio vigente. Si existe un precio vigente, le asigna
`FechaFin`; después agrega el nuevo registro con `FechaInicio` en el mismo
instante y `FechaFin = NULL`.

## Constraints e índice único filtrado

| Regla | Mecanismo |
|---|---|
| Identificar filas | `PRIMARY KEY` |
| Evitar registros huérfanos | `FOREIGN KEY` |
| Evitar SKU duplicado | `UNIQUE` |
| Proteger condiciones de dominio | `CHECK` |
| Completar valores automáticos | `DEFAULT` |
| Permitir solo un precio vigente por producto | Índice único filtrado por `IdProducto` donde `FechaFin IS NULL` |

El DBML actual expresa PK, incrementos y unicidad de `SKU` e `IdProducto`, pero todavía no expresa todos los `CHECK`, `DEFAULT` ni el índice filtrado. La definición exacta debe quedar en el futuro esquema SQL versionado.

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

- Crear o documentar el esquema SQL Server versionado y las migraciones.
- Definir en SQL los `CHECK`, `DEFAULT` y el índice único filtrado.
- Implementar las operaciones de `Inventario`.
- Crear pruebas automatizadas; las pruebas PEG/PEP documentadas hasta ahora son manuales.
