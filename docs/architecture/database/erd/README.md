# ProGear MX - Entity Relationship Diagram

## Modelo actual

El modelo representa inicialmente las entidades relacionadas con productos, inventario e historial de precios.

## Entities

- Producto
- Inventario
- HistorialPrecio

## Relaciones

- Producto 1 : 0..1 Inventario
- Producto 1 : 0..N HistroailPrecio

### Reglas principales

- Un producto puede existir sin registro de inventario.
- Un producto puede existir sin historial de precios.
- Un producto puede tener como máximo un registro de inventario.
- Un producto puede tener múltiples registros históricos de precio.
- Disponible se calcula como Existencia - Reservado.
- FechaFin NULL representa el precio actualmente vigente.
- SKU debe ser único.
