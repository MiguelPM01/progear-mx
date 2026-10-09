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

### Pruebas manuales iniciales — API y middleware

Las notas de progreso desde el arranque registran pruebas de respuestas
`400`, `404` y `500` para el manejo global de errores, además de una request
de creación de producto con datos faltantes. Los endpoints, payloads y
resultados detallados de esos primeros ejercicios no están conservados con
identificadores de caso; se documenta la cobertura general sin atribuirles
comportamientos más específicos.

| Evidencia conservada | Resultado registrado |
|---|---|
| Respuestas de error `400`, `404` y `500` | Se probaron como parte del trabajo inicial de `ErrorHandlingMiddleware`; no se conserva el detalle por endpoint. |
| Alta de producto con datos faltantes | Request probada desde Postman; los datos exactos y su response no están preservados en el registro actual. |

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

### Estado de implementación — 2026-10-09

El vertical actual de productos ya está conectado a SQL Server mediante Entity
Framework Core. La API permite consultar todos los productos, consultar uno por
identificador y crear productos. La creación valida nombre, SKU y SKU duplicado;
el identificador lo genera SQL Server. `HistorialPrecio` cuenta con consultas
GET, consulta por registro y alta/cambio de precio mediante POST. `Inventario`
cuenta con consulta general y específica, entradas, reservas y salidas. La
liberación de reservas sigue pendiente.

### Pruebas manuales de Producto

La evidencia conservada en las notas de progreso incluye creación válida,
identificadores generados `6` y `7`, SKU duplicado, nombre vacío, SKU vacío y
una request incompleta. También se compararon resultados entre Postman,
navegador y consultas directas a SQL Server. Los resultados detallados de la
request incompleta no están preservados, así que no se asigna aquí un código
HTTP ni un código de error a ese caso.

| Caso documentado | Resultado respaldado por código y notas |
|---|---|
| Alta válida por `POST /productos` | `201 Created`; SQL Server asignó los identificadores confirmados `6` y `7`. |
| `Nombre` vacío o solo espacios | `400 Bad Request`, `INVALID_PRODUCT_NAME`. |
| `Sku` vacío o solo espacios | `400 Bad Request`, `INVALID_PRODUCT_SKU`. |
| SKU duplicado | `409 Conflict`, `DUPLICATED_SKU`. |
| Request incompleta | Se probó; el detalle exacto de la respuesta no quedó registrado. |
| Verificación cruzada | Los datos observados en Postman, navegador y consultas SQL Server coincidieron para los casos revisados. |

La API actual no recibe un precio dentro de `POST /productos`; el precio se
registra por separado en `HistorialPrecio`. El handler actual de
`GET /productos/{id}` contiene el literal `PRODUCT_NOT_fOUND` (con `f`
minúscula) para el caso no encontrado; ese contrato debe alinearse con
`PRODUCT_NOT_FOUND` y verificarse en una sesión posterior.

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

El inventario representa las existencias físicas y las unidades apartadas
temporalmente para una operación.

### Datos y reglas

- Un producto puede existir sin registro de inventario y puede tener como
  máximo una fila de inventario.
- `Existencia >= 0` y `Reservado >= 0`.
- `Reservado <= Existencia`.
- `Disponible = Existencia - Reservado`; `Disponible` se calcula, no se
  almacena.
- Un inventario ausente significa que no se registró stock; una fila con
  `Existencia = 0` significa que sí existe un registro sin unidades.

### Operaciones HTTP implementadas — 2026-10-09

En los endpoints de inventario, un `idProducto` no positivo produce
`400 Bad Request` con `INVALID_PRODUCT_ID`, y un producto inexistente produce
`404 Not Found` con `PRODUCT_NOT_FOUND`. Las operaciones que necesitan una fila
de inventario existente responden `404 Not Found` con `INVENTORY_NOT_FOUND` si
no la encuentran; la primera entrada es la excepción porque crea la fila.

#### `GET /inventario`

Devuelve los resúmenes de inventario que tienen producto relacionado mediante
un `Join`. Un producto sin fila de inventario no aparece. La respuesta incluye
`IdProducto`, `Nombre`, `Marca`, `Sku`, `Existencia`, `Reservado` y
`Disponible`.

#### `GET /inventario/{idProducto}`

Devuelve el resumen de un producto y su inventario. Responde `400 Bad Request`
con `INVALID_PRODUCT_ID` para un identificador no positivo; `404 Not Found`
con `PRODUCT_NOT_FOUND` si el producto no existe; y `404 Not Found` con
`INVENTORY_NOT_FOUND` si existe el producto pero no tiene fila de inventario.
En caso válido, responde `200 OK` con `InventarioResponse`.

#### `POST /inventario/{idProducto}/entradas`

Requiere `Cantidad > 0`; responde `400 Bad Request` con
`INVALID_INVENTORY_QUANTITY` si no se cumple. Si el producto aún no tiene
inventario, crea la fila con `Existencia = Cantidad` y `Reservado = 0`. Si ya
existe, suma `Cantidad` a `Existencia` y conserva `Reservado`. Devuelve
`201 Created` con el estado resultante.

#### `POST /inventario/{idProducto}/reservas`

Requiere producto e inventario existentes, `Cantidad > 0` y
`Cantidad <= Disponible`. Si la cantidad supera la disponibilidad, responde
`400 Bad Request` con `INSUFFICIENT_AVAILABLE_STOCK`; si no es positiva,
responde `400 Bad Request` con `INVALID_INVENTORY_QUANTITY`. Una reserva válida
mantiene `Existencia`, incrementa `Reservado` y recalcula `Disponible`. La
respuesta observada es `200 OK` con `InventarioResponse`.

#### `POST /inventario/{idProducto}/salidas`

Requiere producto e inventario existentes, `Cantidad > 0` y
`Cantidad <= Reservado`. Si la cantidad supera lo reservado, responde
`400 Bad Request` con `INSUFFICIENT_RESERVED_STOCK`; si no es positiva,
responde `400 Bad Request` con `INVALID_INVENTORY_QUANTITY`. Una salida válida
reduce `Existencia` y `Reservado` por la misma cantidad, y devuelve `200 OK`
con `InventarioResponse`.

Los handlers validan antes de cambiar entidades y guardar. Las pruebas
registradas confirman que los casos rechazados de salidas no alteraron el
stock. Para otras respuestas de error, el estado se afirma solo cuando se
consultó después con un GET.

#### Liberación de reserva — diseño acordado, pendiente

`POST /inventario/{idProducto}/liberaciones` aún no está implementado. Al
implementarlo, debe exigir `Cantidad > 0` y `Cantidad <= Reservado`. Si se
solicitan 5 unidades y solo hay 3 reservadas, debe devolver `400 Bad Request`
con un `ErrorResponse` coherente, por ejemplo
`INSUFFICIENT_RESERVED_STOCK`, sin modificar el inventario. Una liberación
válida solo reduce `Reservado`; `Existencia` permanece igual y
`Disponible` aumenta por la cantidad liberada. El comportamiento y su código
HTTP de éxito aún no están implementados ni probados.

### Registro de pruebas manuales — Inventario

Los prefijos se reutilizan en módulos diferentes. Cada subsección fija el
módulo de los identificadores, por lo que `PEG-001` o `PEP-001` no son
identificadores globales únicos. Las pruebas aquí registradas fueron manuales.

#### Consultas — PEG, módulo Inventario (`GET /inventario/{idProducto}`)

| ID | Caso | Resultado observado |
|---|---|---|
| PEG-001 | Producto `1` con inventario | `200 OK` con nombre, marca, SKU, existencia, reservado y disponible. |
| PEG-002 | Producto inexistente | `404 Not Found`; la nota de prueba contiene una transcripción inicial con el código mal escrito. El handler actual declara `PRODUCT_NOT_FOUND`. |
| PEG-003 | `idProducto = 0` | `400 Bad Request`, `INVALID_PRODUCT_ID`. |
| PEG-004 | Producto `5` existe, sin fila de inventario | `404 Not Found`, `INVENTORY_NOT_FOUND`. |

#### Entradas — PEI, módulo Inventario

Convención: `PEI` significa **Prueba Endpoint Inventario**.

| ID | Caso | Resultado observado |
|---|---|---|
| PEI-001 | Primera entrada de 10 unidades | `201 Created`; `Existencia = 10`, `Reservado = 0`, `Disponible = 10`. |
| PEI-002 | Entrada adicional de 5 unidades | `201 Created`; `Existencia = 15` sin crear otro registro. |
| PEI-003 | Cantidad `0` | `400 Bad Request`, `INVALID_INVENTORY_QUANTITY`. |
| PEI-004 | Cantidad `-5` | `400 Bad Request`, `INVALID_INVENTORY_QUANTITY`. |
| PEI-005 | `idProducto = -1` | `400 Bad Request`, `INVALID_PRODUCT_ID`. |
| PEI-006 | `idProducto = 0` | `400 Bad Request`, `INVALID_PRODUCT_ID`. |
| PEI-007 | Producto inexistente | `404 Not Found`, `PRODUCT_NOT_FOUND`. |
| PEI-008 | Entrada con una reserva existente | Pendiente; no se conserva un resultado de prueba. |

#### Reservas — PEP, módulo Reservas de Inventario

| ID | Caso | Resultado observado |
|---|---|---|
| PEP-001 | Producto `1`: reservar 4 y luego 7, con 15 de existencia | Ambas requests dieron `200 OK`; el estado pasó de `15/4/11` a `15/11/4` (`Existencia/Reservado/Disponible`). |
| PEP-002 | Producto `4`: reservar las 12 unidades existentes | `200 OK`; `Disponible = 0`. |
| PEP-003 | Producto `3`: reservar más de las 15 unidades disponibles | `400 Bad Request`, `INSUFFICIENT_AVAILABLE_STOCK`. |
| PEP-004 | Cantidad `0` en producto `3` | `400 Bad Request`, `INVALID_INVENTORY_QUANTITY`. |
| PEP-005 | Cantidad `-7` en producto `3` | `400 Bad Request`, `INVALID_INVENTORY_QUANTITY`. |
| PEP-006 | Producto `8` existe, sin fila de inventario | `404 Not Found`, `INVENTORY_NOT_FOUND`. |
| PEP-007 | Producto inexistente `9` | `404 Not Found`, `PRODUCT_NOT_FOUND`. |
| PEP-008 | `idProducto = 0` | `400 Bad Request`, `INVALID_PRODUCT_ID`. |
| PEP-009 | Producto `3`: solicitar reserva de 17 con existencia 15 | `400 Bad Request`, `INSUFFICIENT_AVAILABLE_STOCK`; el GET posterior confirmó `15/0/15`, sin cambios. |

#### Salidas — PEP, módulo Salidas de Inventario

| ID | Caso | Resultado observado |
|---|---|---|
| PEP-001 | Producto `3`: con `15/7/8`, dar salida a 5 | `200 OK`; quedó `10/2/8` (`Existencia/Reservado/Disponible`). |
| PEP-002 | Producto `8`: con `7/7/0`, dar salida a 7 | `200 OK`; quedó `0/0/0`. |
| PEP-003 | Producto `8`: con `7/7/0`, intentar salida de 8 | `400 Bad Request`, `INSUFFICIENT_RESERVED_STOCK`; el GET posterior confirmó `7/7/0`, sin cambios. |
| PEP-004 | Producto `8`: con `7/7/0`, cantidad `0` | `400 Bad Request`; el GET posterior confirmó que el inventario no cambió. |
| PEP-005 | Producto `8`: con `7/7/0`, cantidad `-5` | `400 Bad Request`; el GET posterior confirmó que el inventario no cambió. |

En las notas originales de `PEP-004` y `PEP-005`, el código se transcribió como
`INVLID_INVENTORY_QUANTITY`. El handler actual usa
`INVALID_INVENTORY_QUANTITY`; el status y la conservación del inventario sí
quedaron registrados, pero el código exacto de esas dos responses requiere
reconciliarse con una nueva request.

#### Historial de precios — PEG/PEP, módulo HistorialPrecio

`PEG-001` a `PEG-009`, `PEP-010` a `PEP-015` y la prueba controlada de rollback
se mantienen en la sección 4 de este documento. Sus identificadores pertenecen
a ese módulo y son independientes de las matrices de Inventario.

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

Las reglas siguientes describen el comportamiento requerido del dominio. La
API actualmente implementa crear reservas y confirmar salidas; liberar una
reserva todavía no tiene endpoint.

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
