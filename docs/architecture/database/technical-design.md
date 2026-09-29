# ProGear MX — Documentación técnica de base de datos

## Propósito y estado

Este documento inicia la documentación técnica de `Producto`, `Inventario` e `HistorialPrecio`. El modelo conceptual está en `docs/database/erd/progear-mx.dbml`. La API todavía usa almacenamiento en memoria; SQL Server y EF Core son la siguiente etapa. Por ello, aquí se documenta el diseño y el flujo SQL validado en la sesión del 2026-09-29, sin afirmar que ya exista una migración o un `DbContext` implementado.

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

`@MomentoCambio` se captura una sola vez para que el cierre y la apertura compartan exactamente el mismo instante. `BEGIN TRANSACTION` inicia la operación, `COMMIT` confirma y `ROLLBACK` revierte si una parte falla.

## Trazabilidad

- **Requisito:** un producto no debe tener más de un precio vigente.
- **Modelo:** el precio vigente tiene `FechaFin = NULL`.
- **Protección:** índice único filtrado por `IdProducto` con `FechaFin IS NULL`.
- **Operación:** cerrar con `UPDATE` y crear con `INSERT` dentro de una transacción.
- **Resultado observado:** el precio anterior quedó cerrado y el nuevo quedó vigente.

## Pendientes explícitos

- Crear el esquema SQL Server versionado.
- Definir en SQL los `CHECK`, `DEFAULT` y el índice único filtrado.
- Configurar EF Core, entidades persistentes y `DbContext`.
- Probar desde la API el cambio de precio y sus casos de error.
