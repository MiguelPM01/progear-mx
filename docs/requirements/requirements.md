# ProGear MX — Requirements

## 1. Propósito

ProGear MX es un sistema orientado a la gestión de productos, inventario y ventas para una tienda física de herramientas y productos tecnológicos.

El proyecto busca construir software funcional y profesional, manteniendo trazabilidad entre requerimientos, diseño, implementación, pruebas y documentación.

---

## 2. MVP

El MVP contempla inicialmente:

- Productos
- Inventario
- Clientes
- Promociones
- Ventas
- Pagos

El desarrollo será incremental y cada funcionalidad deberá pasar por:

1. Requerimiento
2. Diseño suficiente
3. Implementación
4. Pruebas
5. Revisión
6. Documentación
7. Commit

---

## 3. Producto

El sistema debe permitir:

- Registrar productos.
- Consultar productos.
- Identificar productos mediante SKU.
- Mantener el nombre del producto.
- Mantener la marca del producto.
- Mantener el historial completo de precios.

### Reglas

- El SKU debe ser único.
- Un producto puede existir aunque todavía no tenga inventario registrado.
- Un producto puede existir aunque todavía no tenga un precio registrado.
- El precio no se almacenará directamente en Producto.
- El precio vigente se obtiene del historial de precios.

### Estado de implementación — 2026-10-06

El vertical actual de productos ya está conectado a SQL Server mediante Entity
Framework Core. La API permite consultar todos los productos, consultar uno por
identificador y crear productos. La creación valida nombre, SKU y SKU duplicado;
el identificador lo genera SQL Server. `HistorialPrecio` cuenta con consultas
GET, consulta por registro y alta/cambio de precio mediante POST. Las
operaciones de `Inventario` todavía están pendientes.

---

## 4. Historial de precios

Cada cambio de precio debe conservarse para mantener trazabilidad.

Un producto puede tener:

- Ningún registro de precio.
- Un registro de precio.
- Múltiples registros históricos.

### Operaciones HTTP implementadas

#### `GET /productos/{id}/precios/historial`

Devuelve la colección completa de registros de precio del producto.

- `400 Bad Request` si `id` no es mayor que cero, con código
  `INVALID_PRODUCT_ID`.
- `404 Not Found` si no existe el producto, con código
  `PRODUCT_NOT_FOUND`.
- `200 OK` con una colección de `HistorialPrecio`; si no hay registros, la
  colección es `[]`.

#### `GET /productos/{id}/precio`

Devuelve el único registro cuyo `FechaFin` es `NULL`, es decir, el precio
vigente.

- `400 Bad Request` si `id` no es mayor que cero, con código
  `INVALID_PRODUCT_ID`.
- `404 Not Found` si no existe el producto, con código
  `PRODUCT_NOT_FOUND`.
- `200 OK` con el registro vigente o `null` si el producto existe pero no tiene
  un precio vigente.

Estas consultas no cambian precios ni crean registros; solo leen el estado
actual del historial.

#### `GET /productos/{id}/precios/{precioId}`

Devuelve un registro del historial que pertenece al producto y al identificador
de precio solicitado.

- `400 Bad Request` si `id` no es mayor que cero, con código
  `INVALID_PRODUCT_ID`.
- `400 Bad Request` si `precioId` no es mayor que cero, con código
  `INVALID_PRICE_ID`.
- `404 Not Found` si no existe el producto, con código `PRODUCT_NOT_FOUND`.
- `404 Not Found` si el registro no existe o pertenece a otro producto, con
  código `PRICE_HISTORY_NOT_FOUND`.
- `200 OK` cuando el registro pertenece al producto solicitado.

#### `POST /productos/{id}/precios`

Registra el primer precio o cambia el precio vigente conservando el historial.

- `400 Bad Request` si `id` no es mayor que cero, si el precio es menor o igual
  que cero (`INVALID_PRODUCT_PRICE`) o si coincide con el precio vigente
  (`DUPLICATED_PRICE`).
- `404 Not Found` si no existe el producto, con código `PRODUCT_NOT_FOUND`.
- `201 Created` cuando se crea el registro y se confirma la operación.
- Si existe un precio vigente, se cierra con `FechaFin`, se crea el nuevo con
  `FechaInicio` en el mismo instante y `FechaFin = NULL`.
- El cierre y la creación se ejecutan dentro de una transacción; `Commit` se
  realiza cuando el flujo termina correctamente y `Rollback` se ejecuta si
  ocurre una excepción.

### Reglas

- Cada registro pertenece a un producto.
- `FechaInicio` indica cuándo comienza la vigencia.
- `FechaFin` indica cuándo termina.
- `FechaFin = NULL` representa el precio actualmente vigente.
- No debe existir más de un precio vigente simultáneamente para un producto.

### Reglas implementadas o validadas en la sesión del 2026-10-06

Estas reglas quedaron expresadas, implementadas y probadas manualmente desde la
API.

- Un cambio de precio debe cerrar el registro vigente y crear un nuevo registro histórico.
- El registro anterior recibe `FechaFin = @MomentoCambio`.
- El nuevo registro recibe `FechaInicio = @MomentoCambio` y `FechaFin = NULL`.
- El cierre y la creación forman una sola transacción; si falla la operación, sus cambios deben poder revertirse con `ROLLBACK`.
- Un índice único filtrado debe impedir más de un registro con `FechaFin IS NULL` para el mismo `IdProducto`.
- El historial puede conservar varios registros con el mismo precio; la regla es la unicidad del precio vigente, no la unicidad histórica del valor.

### Registro de pruebas manuales

Convención: `PEG` significa **Prueba Endpoint GET** y `PEP` significa **Prueba
Endpoint POST**.

| ID | Endpoint | Resultado observado |
|---|---|---|
| PEG-001 | `GET /productos/{id}/precios/historial` | `200 OK` con historial existente. |
| PEG-002 | `GET /productos/{id}/precios/historial` | `200 OK` con `[]` para un producto sin historial. |
| PEG-003 | `GET /productos/{id}/precios/historial` | `404 Not Found` para producto inexistente. |
| PEG-004 | `GET /productos/{id}/precios/historial` | `400 Bad Request` para identificador inválido. |
| PEG-005 | `GET /productos/{id}/precio` | `200 OK` con precio vigente. |
| PEG-006 | `GET /productos/{id}/precio` | `200 OK` con `null` sin precio vigente. |
| PEG-007 | `GET /productos/{id}/precio` | `404 Not Found` para producto inexistente. |
| PEG-008 | `GET /productos/{id}/precio` | `400 Bad Request` para identificador inválido. |
| PEG-009 | `GET /productos/{id}/precios/{precioId}` | `200 OK` con registro perteneciente al producto. |
| PEP-010 | `POST /productos/{id}/precios` | `201 Created` al registrar el primer precio. |
| PEP-011 | `POST /productos/{id}/precios` | `201 Created` al cambiar el precio y conservar el anterior. |
| PEP-012 | `POST /productos/{id}/precios` | `400 Bad Request` al repetir el precio vigente. |
| PEP-013 | `POST /productos/{id}/precios` | `400 Bad Request` para precio menor o igual que cero. |
| PEP-014 | `POST /productos/{id}/precios` | `404 Not Found` para producto inexistente. |
| PEP-015 | `POST /productos/{id}/precios` | `400 Bad Request` para identificador inválido. |

### Prueba controlada de rollback

Partiendo de `$2,100` vigente, se intentó registrar `$2,500` y se provocó una
excepción temporal después de `SaveChangesAsync()` y antes de `CommitAsync()`.
La API respondió `500 Internal Server Error` mediante el middleware. Después de
la operación, `$2,100` continuó vigente y `$2,500` no quedó confirmado. La
condición artificial se eliminó y una prueba normal posterior devolvió
`201 Created`.

---

## 5. Inventario

El inventario representa el estado actual de las existencias de un producto.

### Datos

- Existencia
- Reservado

La disponibilidad se calcula como:

`Disponible = Existencia - Reservado`

### Reglas

- Un producto puede existir sin registro de inventario.
- Un producto puede tener como máximo un registro de inventario.
- `Existencia` no puede ser negativa.
- `Reservado` no puede ser negativo.
- `Reservado` no puede superar la existencia.
- No se almacena `Disponible`; se calcula a partir de los datos existentes.

---

## 6. Ventas

El flujo conceptual de una venta es:

1. El cliente solicita un producto.
2. El sistema consulta el producto.
3. El sistema consulta su disponibilidad.
4. El producto puede reservarse.
5. Se muestra el resumen de la venta.
6. El usuario revisa y confirma.
7. Se procesa el pago.
8. Si el pago es exitoso, la venta se concreta.
9. El inventario se actualiza.

### Reglas

- No se debe vender una cantidad superior a la disponible.
- No debe existir inventario negativo.
- El precio utilizado en la venta debe conservarse.
- La venta se considera concretada después de un pago exitoso.

---

## 7. Reservas

Una venta puede reservar inventario antes de concretarse.

### Reglas

- Una reserva disminuye la disponibilidad.
- Una cancelación libera la reserva.
- Una venta concretada descuenta físicamente la existencia y libera la reserva.
- Las reservas pueden liberarse por expiración.
- Si un pago falla, se permite cambiar el método y reintentar hasta dos veces.
- Después del segundo fallo, la venta se cancela y la reserva se libera.

---

## 8. Métodos de pago iniciales

- Efectivo
- Tarjeta
- Contactless
- CoDi

Para efectivo:

`Cambio = MontoRecibido - Total`

Para métodos no monetarios:

`MontoAcreditado = Total`

---

## 9. Fuera del MVP

Inicialmente quedan fuera:

- Recursos humanos
- Nómina
- Finanzas avanzadas
- Reportes avanzados
- Power BI
- IA
- IoT
- Gestión avanzada de proveedores
- Módulo completo de compras

Estos elementos podrán evaluarse en futuras versiones.
