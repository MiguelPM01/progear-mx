# Requerimientos del frontend — ProGear MX

**Estado revisado:** 14 de octubre de 2026

**Base:** código actual bajo `frontend/src/` y evidencia visual compartida en
la sesión. Los estados indican implementación en el código, no una nueva
ejecución de pruebas.

## Alcance actual

El frontend tiene una estructura inicial de dashboard con rutas declaradas y
páginas de presentación para las áreas principales. No gestiona datos ni se
conecta con la API. La documentación de su estructura está en
[`docs/frontend/design.md`](../frontend/design.md).

## Estado de las decisiones

| Área | Requerimiento o decisión registrada | Estado |
|---|---|---|
| Layout | Mostrar `Header` en la parte superior y, debajo, `Sidebar` junto al contenido de las rutas. | Implementado en `App.tsx`; la Sidebar y las rutas comparten un contenedor flex horizontal. |
| Dashboard | La ruta `/` muestra el Panel Principal y un mensaje de bienvenida. | Implementado como página inicial de presentación. |
| Productos | La ruta `/productos` muestra la página de gestión de productos. | Ruta y encabezado implementados; flujo de gestión pendiente. |
| Inventario | La ruta `/inventario` muestra la página de gestión de inventario. | Ruta y encabezado implementados; flujo de gestión pendiente. |
| Precios | La ruta `/precios` muestra el historial de precios. | Ruta y encabezado implementados; flujo de gestión pendiente. |
| Ruta desconocida | Las rutas no declaradas muestran una página 404. | Implementado mediante la ruta comodín `*`. |
| Header | Mostrar la marca ProGear MX en la parte superior. | Implementado. El espacio reservado para un control todavía es un marcador. |
| Sidebar | Mostrar la marca y ofrecer navegación a Productos, Inventario y Precios. | Implementado con un placeholder de logo y enlaces `NavLink`. |
| Enlace activo | Diferenciar visualmente la ruta activa de las demás. | Clases condicionales implementadas: activo con `bg-primary text-white`; inactivo con `text-secondary` y hover. Falta comprobar visualmente cada estado. |
| Tema | Mantener el tema oscuro con superficie grafito, texto claro y acentos azul y rojo, usando tokens `@theme` de Tailwind. | Tokens declarados en `frontend/src/index.css`; falta conciliar `--color-primary` con el resumen anterior y verificar la carga de fuentes. |
| Control de Sidebar | Permitir alternar u ocultar la Sidebar desde el área del Header. | Planeado; no hay interacción implementada y faltan decisiones de activación y comportamiento adaptable. |
| Integración | Cargar y modificar datos usando la API de ProGear MX. | Pendiente. |

## Límites conocidos

- Las páginas de productos, inventario y precios muestran encabezados, no
  formularios ni datos conectados.
- `Sidebar` no tiene un enlace a `/`; el dashboard no activa ninguno de sus
  enlaces actuales.
- El Header aún no implementa el control de visibilidad de la Sidebar.
- Los estilos del estado activo existen en el código, pero no se registró una
  comprobación visual de las rutas en esta actualización.
- Rajdhani e Inter están declaradas en CSS; su carga efectiva queda por
  verificar.
- El valor de `--color-primary` en el código es `#1677E8`; una nota anterior
  registró `#2589FF`. La diferencia sigue pendiente de decisión.
