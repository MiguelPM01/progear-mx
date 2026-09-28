# Glosario acumulativo — ProGear MX

| Término | Significado en el proyecto |
|---|---|
| API | Interfaz mediante la cual componentes de software se comunican. |
| REST | Estilo para diseñar APIs usando recursos, HTTP y operaciones sobre ellos. |
| HTTP | Protocolo utilizado para la comunicación entre cliente y servidor. |
| Request | Petición que el cliente envía al servidor. |
| Response | Respuesta que el servidor devuelve al cliente. |
| Endpoint | Punto de la API que atiende una operación mediante una ruta y un método HTTP. |
| GET | Método HTTP usado principalmente para consultar información. |
| POST | Método HTTP usado principalmente para enviar información o crear un recurso. |
| HTTP Request Pipeline | Recorrido que sigue una request dentro de ASP.NET Core hasta generar una response. |
| Middleware | Componente del pipeline que ejecuta una responsabilidad sobre requests/responses y puede continuar o intervenir. |
| `ErrorHandlingMiddleware` | Middleware de ProGear MX que captura excepciones del procesamiento y construye una respuesta controlada. |
| `RequestDelegate` | Representación del siguiente componente del pipeline. |
| `_next` | Referencia al siguiente componente que recibe la request. |
| `HttpContext` | Contexto completo de la request actual, incluyendo `Request` y `Response`. |
| `HttpContext.Request` | Información de la petición recibida: método, ruta, headers y body. |
| `HttpContext.Response` | Respuesta que el servidor prepara para devolver al cliente. |
| `StatusCode` | Código HTTP que representa el resultado de la response. |
| `ContentType` | Tipo de contenido de la response, por ejemplo `application/json`. |
| `Response.Body` | Cuerpo que se envía como parte de la response. |
| Model Binding | Proceso mediante el cual ASP.NET Core obtiene datos de una request y los proporciona como parámetros u objetos. |
| DTO | Objeto usado para transportar datos entre partes de una aplicación. |
| `CrearProductoRequest` | DTO que representa los datos esperados para crear un producto. |
| Serialización | Conversión de un objeto de la aplicación a un formato como JSON. |
| Deserialización | Conversión de JSON u otro formato a un objeto de la aplicación. |
| `JsonSerializer` | Herramienta de .NET para serializar y deserializar JSON. |
| `try/catch` | Mecanismo de C# para intentar ejecutar código y capturar excepciones. |
| Exception | Situación excepcional que ocurre durante la ejecución. |
| `ErrorResponse` | Modelo usado para representar un error de forma controlada, con `Exito`, `Codigo` y `Mensaje`. |
| SKU | Identificador usado para distinguir un producto en el inventario. |
| `400 Bad Request` | Respuesta usada cuando la request contiene datos inválidos o no puede procesarse correctamente. |
| `401 Unauthorized` | Respuesta asociada con una request que requiere autenticación válida. |
| `403 Forbidden` | Respuesta asociada con un cliente autenticado que no tiene permisos suficientes. |
| `404 Not Found` | Respuesta usada cuando no se encuentra el recurso solicitado. |
| `409 Conflict` | Respuesta usada cuando existe un conflicto con el estado actual del recurso. |
| `500 Internal Server Error` | Respuesta usada para un error interno del servidor. |
| Registro del middleware | Incorporación del middleware al pipeline, por ejemplo con `app.UseMiddleware<T>()`. |
| Entity | Elemento del dominio que necesita ser representado y almacenado, Ejemplos `Producto, Inventario, HistorialPrecio`. |
| Atributo | Dato que describe una cantidad, ejemplos `Producto.Nombre, Producto.Marca, Producto.SKU` .|
|Primary Key (PK) | Campo que identifica de manera única un registro dentro de una tabla, Ejemplo `Producto.Id`. |
| Foreign Key (FK) | Campo que referencia la Primary Key de otra tabla y permite establecer una relación entre entidades, Ejemplo `Inventario.IdProducto -> Producto.Id`. |
| Cardinalidad | Describe cuántos registros de una entidad pueden relacionarse una con otra. |
| `0..1` | Cero o uno. |
| `1..1` | Exactamente uno. |
| `0..N` | Cero o muchos. |
| `1..N` | Uno o muchos. |
| ERD (Entity Relationship Diagram ) | Diagrama que representa entidades, atributos y relaciones de una base de datos.
| DBML (Database Markup Language) | Lenguaje utilizado por herramientas como DBDiagram para describir estructuras de bases de datos y sus relaciones. |
| Inventario | Representa el estado de las existencias físicas de un producto. |
| Existencia | Cantidad física registrada de un producto.
| Reservado | Cantidad de unidades que forman parte de la existencia pero están comprometidas temporalmente para una operación, como una venta pendiente. |
| Disponible | Cantidad que puede ser ofrecida para nuevas operaciones. se calcula: `Disponible = Existencia - Reservado` no se almacena directamente. |
| Historial de precios | Conjunto de registros que permite conservar los diferentes precios que ha tenido un producto a través del tiempo. |
| Precio Vigente | Precio cuyo período de vigencia todavía no ha terminado. |
| NULL | Ausencia de un valor. |
| Staging | Área de Git donde se seleccionan los cambios que formarán parte del siguiente commit. |
| Commit | Registro de un conjunto de cambios dentro del repositorio local de Git.
| Push | Operación que publica commits locales en el repositorio remoto. |
| Working Directory | Estado de los archivos del proyecto sobre los que estamos trabajando antes de agregarlos al staging. |
