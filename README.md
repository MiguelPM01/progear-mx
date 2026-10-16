# ProGear MX

API REST para gestionar el catálogo de una tienda especializada en herramientas.

ProGear MX es un proyecto de aprendizaje práctico y portafolio. Su primer
vertical es Product Management: modelar productos, consultarlos y validar su
creación mientras se construye una base sólida de ingeniería de software.

## Estado actual — 2026-10-15

### Backend

La API REST está construida con ASP.NET Core Minimal API, Entity Framework
Core y SQL Server.

El backend incluye operaciones para consultar y registrar productos,
consultar el historial de precios, registrar cambios de precio y gestionar
operaciones de inventario implementadas.

Las reglas de inventario distinguen entre existencia física, unidades
reservadas y disponibilidad:

`Disponible = Existencia - Reservado`

La documentación de requerimientos contiene la matriz de pruebas y el estado
de las operaciones implementadas.

### Frontend

Se inició la interfaz web mediante React, TypeScript, Vite y Tailwind CSS.

El frontend cuenta con:

- Navegación mediante React Router.
- Sidebar con enlaces a Inicio, Productos, Inventario y Precios.
- Header con botón Menú.
- Página inicial con un dashboard.
- Componente reutilizable `KpiCard`.
- Cuatro tarjetas KPI para productos registrados, existencias físicas,
  unidades reservadas y disponibilidad.
- Iconos de `lucide-react`.
- Identidad visual basada en colores y tipografías personalizados.
- Cuadrícula KPI con clases responsivas de Tailwind.

Los valores mostrados en el dashboard son ilustrativos. El frontend todavía
no está conectado a la API para obtener estos indicadores.

Las páginas de Productos, Inventario y Precios tienen navegación configurada,
pero aún requieren la implementación de sus interfaces funcionales.

## Tecnologías

### Backend

- .NET 10
- C#
- ASP.NET Core Minimal API
- Entity Framework Core
- SQL Server

### Frontend

- React
- TypeScript
- Vite
- Tailwind CSS
- React Router
- Lucide React

### Ingeniería y documentación

- Git y GitHub
- Postman
- Markdown
- Documentación de requerimientos, diseño, pruebas y aprendizaje

## Identidad visual

La interfaz utiliza una paleta oscura con azul como color principal y gris
plateado como color secundario.

Las tipografías principales son Rajdhani para títulos y valores destacados,
e Inter para textos generales y navegación.

Los tokens de color y las fuentes se configuran en `frontend/src/index.css`.

## Enfoque del proyecto

El proyecto prioriza construir un vertical pequeño y comprensible, registrar
el razonamiento y agregar complejidad solo cuando una necesidad concreta la
justifique.

La metodología de trabajo es incremental:

1. Entender el problema.
2. Definir requisitos y reglas de negocio.
3. Diseñar una solución suficiente.
4. Implementar.
5. Probar.
6. Revisar y depurar.
7. Documentar.
8. Registrar los cambios mediante Git.

El objetivo es desarrollar criterio técnico y autonomía, no limitarse a
copiar código o completar tutoriales.

## Documentación

- `docs/requirements/requirements.md`: requerimientos y matriz de pruebas.
- `docs/notes/learning-notebook.md`: registro de sesiones y aprendizajes.
- `docs/glossary.md`: glosario acumulativo.
- `docs/progress.md`: avance general.
- Documentación de diseño: decisiones de arquitectura y diseño visual.
