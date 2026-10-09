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
| SQL Server | Sistema gestor de bases de datos relacionales usado como destino de persistencia de ProGear MX. |
| T-SQL | Dialecto de SQL utilizado por SQL Server para declarar variables y controlar operaciones. |
| Constraint / restricción | Regla que limita los datos permitidos en una tabla. |
| `NOT NULL` | Restricción que exige que una columna tenga un valor. |
| `UNIQUE` | Restricción que impide valores duplicados en una columna o combinación definida. |
| `CHECK` | Restricción que exige que un valor cumpla una condición. |
| `DEFAULT` | Valor que SQL Server asigna automáticamente cuando no se proporciona uno. |
| `INDEX` / índice | Estructura que ayuda a buscar datos y puede reforzar una regla de unicidad. |
| Índice único filtrado | Índice `UNIQUE` aplicado solo a filas que cumplen un filtro; aquí permite una sola fila con `FechaFin IS NULL` por producto. |
| Transaction / transacción | Operación compuesta cuyos cambios se confirman juntos o se revierten juntos. |
| `BEGIN TRANSACTION` | Inicia una transacción. |
| `COMMIT TRANSACTION` | Confirma los cambios de una transacción. |
| `ROLLBACK TRANSACTION` | Revierte los cambios de una transacción cuando la operación falla. |
| `DECLARE` | Instrucción T-SQL para declarar una variable local. |
| Variable T-SQL | Valor temporal identificado por un nombre, normalmente con prefijo `@`, que puede reutilizarse durante la operación. |
| `DATETIME2` | Tipo de SQL Server para almacenar fecha y hora con precisión fraccionaria. |
| `SYSDATETIME()` | Función que obtiene la fecha y hora actual del servidor SQL. |
| `UPDATE` | Instrucción que modifica filas existentes. |
| `INSERT` | Instrucción que agrega nuevas filas. |
| `async` | Modificador de C# que permite que un método realice operaciones asíncronas y pueda usar `await`. |
| `await` | Expresión de C# que espera el resultado de una operación asíncrona. |
| `DbContext` | Contexto de EF Core que conecta los modelos de la aplicación con la base de datos y coordina la persistencia. |
| `DbSet` | Colección de EF Core que representa una entidad y permite consultar o seguir sus registros. |
| `OnModelCreating` | Método donde se configura cómo las entidades se mapean a tablas, claves, relaciones y propiedades de la base de datos. |
| Entity Framework Core (EF Core) | Framework de .NET que permite trabajar con la base de datos usando modelos y expresiones de C#. |
| `ToListAsync()` | Método de EF Core que ejecuta una consulta asíncrona y devuelve sus resultados como una lista. |
| `FindAsync()` | Método de EF Core que busca una entidad por su clave primaria de forma asíncrona. |
| `AnyAsync()` | Método de EF Core que comprueba de forma asíncrona si existe al menos un registro que cumple una condición. |
| `Add()` | Operación de EF Core que agrega una entidad al seguimiento del `DbContext` para prepararla para persistencia. |
| `SaveChangesAsync()` | Método de EF Core que persiste de forma asíncrona los cambios seguidos por el `DbContext`. |
| `IDENTITY` | Configuración de SQL Server que genera automáticamente valores numéricos para una columna, como el `Id` de Producto. |
| Traducción de expresiones a SQL | Proceso mediante el cual EF Core convierte expresiones de C# en consultas SQL que ejecuta la base de datos. |
| `HasKey` | Configuración de EF Core que indica la clave primaria de una entidad. |
| `HasPrecision(10, 2)` | Configuración de EF Core que define precisión 10 y 2 decimales para un valor decimal, como `Precio`. |
| `HasOne` / `WithMany` | Configuración de EF Core que expresa una relación de uno a muchos entre entidades. |
| `HasForeignKey` | Configuración de EF Core que indica la propiedad que funciona como clave foránea. |
| `ToListAsync()` | Ejecuta una consulta asíncrona y materializa todos los resultados como una lista; si no hay filas, la lista es `[]`. |
| `SingleOrDefaultAsync()` | Ejecuta una consulta que espera cero o un resultado; devuelve `null` sin filas y falla si encuentra más de uno. |
| Colección vacía `[]` | Respuesta que representa una lista sin elementos. |
| Recurso único `null` | Respuesta que indica que no existe el recurso único solicitado, sin confundirlo con una colección vacía. |
| Debugging basado en evidencia | Investigar comparando código, consulta y respuesta observada antes de modificar la implementación. |
| Scope / alcance | Límite dentro del cual una variable, operación o regla es válida. |
| Atomicidad | Propiedad por la que una operación compuesta se confirma completa o se revierte completa. |
| Commit de transacción | Confirmación definitiva de los cambios realizados dentro de una transacción. |
| Rollback | Reversión de los cambios de una transacción que no pudo completarse. |
| BeginTransactionAsync | Método que inicia una transacción de base de datos de forma asíncrona. |
| CommitAsync | Método que confirma una transacción de forma asíncrona. |
| RollbackAsync | Método que revierte una transacción de forma asíncrona. |
| Prueba controlada | Prueba que provoca deliberadamente una condición conocida para comprobar un comportamiento. |
| PEG | Convención de prueba: Prueba Endpoint GET. |
| PEP | Convención de prueba: Prueba Endpoint POST. |
| PEI | Convención de prueba: Prueba Endpoint Inventario. |
| JOIN | Operación que combina registros de dos conjuntos usando una clave relacionada. |
| INNER JOIN | Tipo de JOIN que devuelve únicamente registros con correspondencia en ambos conjuntos. |
| Request DTO | DTO que representa los datos que la API recibe en una request. |
| Response DTO | DTO que representa los datos que la API devuelve en una response. |
| InventarioResponse | DTO de salida que combina datos de Producto, Inventario y Disponible calculado. |
| CantidadRequest | DTO de entrada reutilizable que recibe la cantidad de una operación de inventario. |
| Reservation / reserva | Aparta temporalmente parte de `Disponible` para una operación. No reduce la existencia física; aumenta `Reservado`. |
| Release / liberación de reserva | Devuelve unidades reservadas a la disponibilidad. Reduce `Reservado` y conserva `Existencia`; el endpoint de ProGear MX está pendiente. |
| Stock issue / salida de inventario | Confirma la salida de unidades reservadas. Reduce `Existencia` y `Reservado` por la misma cantidad. |
| `INSUFFICIENT_AVAILABLE_STOCK` | Error de inventario cuando la cantidad solicitada para una reserva supera `Disponible`. |
| `INSUFFICIENT_RESERVED_STOCK` | Error de inventario cuando una salida o futura liberación solicita más unidades que las reservadas. |
| Invariant / invariante | Condición que debe mantenerse siempre en los datos; aquí `Existencia >= 0`, `Reservado >= 0` y `Reservado <= Existencia`. |
| No mutation on rejected request | Regla por la que una solicitud inválida devuelve `ErrorResponse` antes de guardar cambios; las pruebas de salidas confirmaron que las cantidades no cambiaron en los rechazos revisados. |
