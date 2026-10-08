# ProGear MX — Cuaderno de aprendizaje técnico

> Registro personal de lo que voy entendiendo mientras construyo ProGear MX. La explicación parte de mis palabras; el formato y algunas precisiones técnicas fueron ordenados con ayuda del asistente. No es un manual genérico: conserva el camino, las dudas y los errores reales.

## Cómo usar este cuaderno

Cada entrada conserva, cuando está disponible: término en inglés, explicación sencilla, mi razonamiento, ejemplo corto, error o duda y qué aprendí.

---

## Sesión — 21 de septiembre de 2026

### Git: flujo básico

**Términos en inglés:** Git, repository, working tree, staging area, commit, push, branch, remote.

**Explicación sencilla:** Git guarda el historial. El working tree es donde están mis archivos; git add prepara cambios en el staging area; git commit los registra localmente; git push envía esos commits al remoto de GitHub.

**Cómo lo razoné:** Lo comparé con GitHub web: creo o modifico un archivo, preparo el cambio, escribo una descripción y hago el commit. Luego entendí que la terminal separa esos pasos.

**Ejemplo:**

~~~powershell
git status
git add README.md
git commit -m "docs: add project README"
git push -u origin main
~~~

**Error o duda:** Pensé que git add ya hacía el cambio en el repositorio. También apareció el aviso de que el upstream no existía porque el remoto estaba vacío.

**Qué aprendí:** git add prepara; commit registra localmente; push envía a GitHub. working tree clean significa que no hay cambios pendientes y up to date que local y remoto están sincronizados.

### .NET SDK

**Términos en inglés:** SDK (Software Development Kit), runtime, command, version.

**Explicación sencilla:** El SDK es el kit para desarrollar con .NET: crear proyectos, compilar, ejecutar y probar. El runtime sirve principalmente para ejecutar aplicaciones construidas.

**Cómo lo razoné:** El mensaje de dotnet --version decía que no encontraba SDKs. Aunque no sé mucho inglés, pude leerlo y deducir que faltaba la herramienta para desarrollar.

**Ejemplo:**

~~~powershell
winget --version
winget install Microsoft.DotNet.SDK.10
dotnet --version
~~~

**Resultado:** Se verificó el SDK 10.0.401.

**Error o duda:** Confundí mensajes sobre el certificado HTTPS con la creación del proyecto. También asocié dotnet new con crear un entorno completo.

**Qué aprendí:** dotnet --version muestra la versión del SDK. dotnet new crea algo usando una plantilla.

### Project y Solution

**Términos en inglés:** project, solution, template, .csproj, .slnx.

**Explicación sencilla:** Un project contiene una aplicación o componente compilable: código, configuración, dependencias y un .csproj. Una solution agrupa proyectos relacionados; no es donde vive directamente todo el código.

**Cómo lo razoné:** Lo comparé con Canva: dotnet new es elegir una plantilla, el project es mi CV concreto y la solution es el conjunto de proyectos relacionados.

**Ejemplo:**

~~~powershell
dotnet new sln -n ProGear
dotnet new webapi -n ProGear.Api
dotnet sln ProGear.slnx add ProGear.ApiProGear.Api.csproj
dotnet sln ProGear.slnx list
~~~

**Error o duda:** Pensé que crear ProGear.Api con dotnet new webapi lo agregaba automáticamente a ProGear.slnx. Primero se creó el proyecto y después hubo que registrarlo.

**Qué aprendí:** ProGear.slnx → ProGear.Api → ProGear.Api.csproj → código C#. La solution agrupa; el project es donde se desarrolla y compila.

### build y run

**Términos en inglés:** build, compile, run, restore, output.

**Explicación sencilla:** dotnet build restaura dependencias, compila y genera archivos de salida, como un .dll. dotnet run ejecuta el proyecto y deja la aplicación funcionando.

**Cómo lo razoné:** build responde “¿puedo construir esta aplicación?” y run responde “ejecútala”.

**Ejemplo:**

~~~powershell
dotnet build
dotnet run --project ProGear.Api
~~~

**Error o duda:** Pensé que dotnet run establecía la relación entre proyecto y solution. Esa relación ya la había hecho dotnet sln ... add ...

**Qué aprendí:** Compilar correctamente no significa que la API ya esté ejecutándose. El flujo es construir → ejecutar → probar → corregir.

### Program.cs: builder, Build(), app y Run()

**Términos en inglés:** builder, build, application, pipeline, middleware, endpoint.

**Explicación sencilla:** builder prepara y configura; builder.Build() construye; app representa la aplicación construida; app.Use... y app.Map... configuran su comportamiento; app.Run() inicia y mantiene el servidor.

**Cómo lo razoné:** Primero dije que builder construía la API y que app era la aplicación construida. La precisión fue separar preparar, construir, configurar y ejecutar.

**Ejemplo:**

~~~csharp
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/productos", () => "ProGear");
app.Run();
~~~

**Error o duda:** La plantilla incluía IsDevelopment(), MapOpenApi() y UseHttpsRedirection(). No entendía todo el pipeline, así que lo fuimos leyendo por partes.

**Qué aprendí:** El orden mental es preparar → construir → configurar comportamiento → ejecutar.

### localhost, endpoint y respuesta HTTP

**Términos en inglés:** localhost, endpoint, route, request, response, HTTP GET, 404 Not Found, port.

**Explicación sencilla:** localhost es mi propia computadora. Un endpoint combina un método HTTP y una ruta que la API sabe atender. El puerto indica dónde escucha el servidor.

**Cómo lo razoné:** Lo comparé con un servicio de Node.js o React corriendo en una terminal.

**Ejemplo:**

~~~text
GET http://localhost:5224/weatherforecast
~~~

~~~csharp
app.MapGet("/weatherforecast", () => datos);
~~~

**Error o duda:** La raíz http://localhost:5224/ no tenía endpoint. /weatherforecast sí funcionó. /weatherforecast/123 devolvió 404 porque esa ruta no estaba registrada.

**Qué aprendí:** 404 Not Found puede significar que no existe una ruta coincidente. Más adelante /products/{id} puede tener ruta válida pero no encontrar el recurso.

### Advertencia de HTTPS

**Términos en inglés:** HTTPS redirection, warning, port, middleware.

**Explicación sencilla:** El middleware intentó redirigir HTTP a HTTPS, pero no pudo determinar el puerto HTTPS.

**Cómo lo razoné:** La terminal decía que escuchaba en http://localhost:5224, así que entendí que la API sí había arrancado.

**Ejemplo:**

~~~text
Now listening on: http://localhost:5224
Failed to determine the https port for redirect.
~~~

**Error o duda:** Pensé que la advertencia significaba que la API había fallado.

**Qué aprendí:** Una advertencia no necesariamente detiene la aplicación.

### C# class, object y properties

**Términos en inglés:** class, object, instance, property, getter, setter.

**Explicación sencilla:** Una class es un molde. Un object es una instancia concreta. Una property describe un dato; get permite obtenerlo y set asignarlo o modificarlo.

**Cómo lo razoné:** Primero lo expresé como pseudocódigo: una clase Producto con nombre, sku y precio. Luego entendí que dentro de la clase había propiedades, no objetos.

**Ejemplo:**

~~~csharp
public class Producto
{
    public string Nombre { get; set; }
    public string Sku { get; set; }
    public decimal Precio { get; set; }
}

var producto = new Producto
{
    Nombre = "Taladro inalámbrico",
    Sku = "TAL-058",
    Precio = 1299.00m
};
~~~

**Error o duda:** Mezclé sintaxis JSON con C# y usé new[] en lugar de new Producto.

**Qué aprendí:** La clase define la estructura; new Producto crea un objeto; las propiedades reciben sus valores.

### decimal para dinero

**Términos en inglés:** decimal, float, double, monetary value, precision.

**Explicación sencilla:** decimal representa valores decimales con precisión apropiada para dinero, como precios, impuestos y descuentos.

**Cómo lo razoné:** Al principio pensé en float porque el precio tiene decimales. Después entendí que un precio es dinero y la precisión importa.

**Ejemplo:**

~~~csharp
public decimal Precio { get; set; }
var precio = 1299.00m;
~~~

**Error o duda:** Propuse truncar siempre a dos decimales. Entendí que truncar o redondear es una regla de negocio pendiente.

**Qué aprendí:** La m indica un literal decimal. La precisión interna, los decimales mostrados, el redondeo y los impuestos son decisiones futuras.

### List<Producto>, Add() y AddRange()

**Términos en inglés:** list, collection, item, add, add range.

**Explicación sencilla:** List<Producto> es una colección de objetos Producto. Add() agrega un elemento; AddRange() agrega varios elementos.

**Cómo lo razoné:** Necesitábamos una lista donde pudiéramos acceder a los productos. Producto es el tipo, producto una instancia, productos la colección y Add() una operación de la colección.

**Ejemplo:**

~~~csharp
var productos = new List<Producto>();
productos.Add(producto);

var otrosProductos = new List<Producto>
{
    taladro,
    esmeril,
    llaveImpacto
};
~~~

**Error o duda:** Intenté productos.Producto(producto) y productos.Add(producto, esmeril, llaveImpacto, multimetro). Add() recibe un solo Producto; para varios se inicializa la lista o se usa AddRange().

**Qué aprendí:** La lista mantiene productos en memoria. Al cerrar el programa desaparecen; por eso JSON será el siguiente paso.

### Índices y Count

**Términos en inglés:** index, count, zero-based index, collection.

**Explicación sencilla:** Las listas comienzan en el índice 0. Count indica cuántos elementos hay.

**Cómo lo razoné:** Con cinco productos los índices son 0, 1, 2, 3 y 4; podemos acceder a productos[0], productos[1], etc.

**Ejemplo:**

~~~csharp
for (int i = 0; i < productos.Count; i++)
{
    Console.WriteLine(productos[i].Nombre);
}
~~~

**Error o duda:** Mezclé la estructura de for dentro de foreach: foreach(int i = 0; i < productos.Count; i++).

**Qué aprendí:** En un for controlo índice, condición e incremento. i < productos.Count evita intentar productos[5].

### foreach

**Términos en inglés:** foreach, loop, current item, collection.

**Explicación sencilla:** foreach dice “por cada elemento de esta colección, haz esto”. No necesito manejar índices manualmente.

**Cómo lo razoné:** Lo describí como recorrer la lista para saber qué contiene. En cada vuelta, producto representa al elemento actual.

**Ejemplo:**

~~~csharp
foreach (var producto in productos)
{
    Console.WriteLine(producto.Nombre);
}
~~~

**Error o duda:** Mezclé for y foreach declarando i, una condición e i++ dentro de foreach.

**Qué aprendí:** foreach recorre directamente los elementos. var no significa que C# ignore el tipo: el compilador deduce que producto es Producto.

### namespace y using

**Términos en inglés:** namespace, using directive, scope.

**Explicación sencilla:** Un namespace es un espacio lógico donde viven nombres de clases. using permite utilizar nombres de ese espacio.

**Cómo lo razoné:** Pensé que Producto estaba subrayado porque había que importarlo. Después vimos que había que comprobar el namespace y la ubicación del archivo.

**Ejemplo:**

~~~csharp
// Producto.cs
namespace ProGear.Api;
public class Producto { }

// Program.cs
using ProGear.Api;
~~~

**Error o duda:** Producto no se encontraba porque Producto.cs había quedado fuera de la carpeta/proyecto ProGear.Api. Después se corrigieron namespace y using.

**Qué aprendí:** using no crea un namespace ni incorpora un archivo al proyecto. El archivo debe pertenecer al proyecto que se compila.

### Nullable reference types: string y string?

**Términos en inglés:** nullable reference type, non-nullable, null, warning.

**Explicación sencilla:** Con nullable habilitado, string expresa que la referencia no debería contener null; string? expresa que sí puede contener null. null y "" no son lo mismo.

**Cómo lo razoné:** Pensé que string significaba que el texto nunca podía estar vacío. La precisión fue entender que la advertencia se refiere a null.

**Ejemplo:**

~~~csharp
public string Nombre { get; set; }
public string? Descripcion { get; set; }
~~~

**Error o duda:** Nombre y Sku producían CS8618. Poner ? quitaría la advertencia, pero contradice la regla de que el producto necesita esos datos.

**Qué aprendí:** string no garantiza que el texto no sea ""; expresa una referencia no anulable. La decisión técnica debe respetar el dominio.

### required y reglas de negocio

**Términos en inglés:** required, initialization, business rule, valid state.

**Explicación sencilla:** required indica que una propiedad debe proporcionarse al inicializar un objeto.

**Cómo lo razoné:** Concluí que necesitamos nombre, SKU y precio para agregar un producto. Entendí que required expresa una regla, no solo quita una advertencia.

**Ejemplo:**

~~~csharp
public required string Nombre { get; set; }
public required string Sku { get; set; }
~~~

**Error o duda:** Pregunté por qué Precio no daba la misma advertencia. decimal es un tipo de valor con 0 predeterminado; eso puede ser válido para C# pero inválido para el negocio.

**Qué aprendí:** Hay diferencia entre un estado válido para el lenguaje y un estado válido para el dominio.

### Error de ubicación de Producto.cs y debugging

**Términos en inglés:** debugging, compiler error, warning, project file, diagnosis.

**Explicación sencilla:** Debugging es investigar con evidencia: leer el error, revisar archivos, formular una hipótesis, probarla y comprobar de nuevo.

**Cómo lo razoné:** Revisé el directorio y descubrí que Producto.cs estaba fuera de ProGear.Api. Al moverlo, desapareció el error principal y quedaron advertencias de nulabilidad.

**Ejemplo:**

~~~text
CS0246: no se encontró Producto
        ↓ revisar estructura
Producto.cs estaba fuera de ProGear.Api
        ↓ corregir ubicación
build correcto con advertencias CS8618
~~~

**Error o duda:** Primero sospeché de using o namespace. La causa real era estructural.

**Qué aprendí:** Un error del compilador puede tener una causa de ubicación o configuración, no solo una línea mal escrita. También distinguí error de advertencia.

### Warning vs error

**Términos en inglés:** warning, error, compiler, build succeeded.

**Explicación sencilla:** Un error impide compilar. Una advertencia avisa de algo posiblemente problemático y puede permitir ejecutar.

**Cómo lo razoné:** Cuando Producto.cs estaba fuera no se reconocía Producto. Después el proyecto compiló con dos advertencias sobre Nombre y Sku.

**Error o duda:** Pensé que cualquier mensaje del editor significaba que la aplicación estaba rota.

**Qué aprendí:** No hay que borrar una advertencia sin entenderla; primero se decide si expresa un problema real.

---

## Próximo tema: JSON y deserialización

Los cinco productos se crearon manualmente, se guardaron en una List<Producto> y se recorrieron con foreach. Eso funciona durante la ejecución, pero al cerrar el programa los datos desaparecen.

**Términos en inglés:** JSON, serialize, deserialize, persistence.

**Explicación sencilla:** JSON representa datos estructurados como texto. Deserializar será convertir JSON en objetos C#; serializar será convertir objetos C# en JSON.

**Mi razonamiento:** “Si no vamos a escribir manualmente los cinco productos, tenemos que traducir los datos al tipo de datos que nuestro programa acepte.” Pensé en un “intérprete” o “traductor”.

**Flujo pendiente:**

~~~text
productos.json
      ↓
deserialización
      ↓
List<Producto>
      ↓
foreach / API
~~~

**Ejemplo de destino:**

~~~json
[
  {
    "nombre": "Taladro inalámbrico",
    "sku": "TAL-058",
    "precio": 1299.00
  }
]
~~~

**Error o duda registrada:** En un intento mezclé sintaxis JSON con C# y aparecieron errores de comillas y diferencias ortográficas. Todavía no se ha implementado la lectura del archivo.

**Qué aprenderé después:** Crear productos.json, conocer System.Text.Json, deserializar a List<Producto> y comprobar que la API trabaje con datos persistidos sin crear cada objeto manualmente.

---

## Glosario de inglés técnico

| English | Significado |
|---|---|
| build / run | construir/compilar / ejecutar |
| warning / error | advertencia / error |
| namespace | espacio de nombres |
| class / object / instance | clase / objeto / instancia |
| property / get / set | propiedad / obtener / asignar |
| list / collection / item | lista / colección / elemento |
| index / count | índice / cantidad |
| required / nullable | requerido / puede aceptar null |
| endpoint / route | punto de acceso / ruta |
| request / response | petición / respuesta |
| localhost / port | computadora local / puerto |
| repository / branch | repositorio / rama |
| working tree / staging area | archivos de trabajo / área de preparación |
| commit / push | registro / enviar al remoto |
| SDK | kit de desarrollo |
| serialize / deserialize | convertir a JSON / convertir desde JSON |
| persistence | persistencia |
| working directory / working tree | carpeta con los archivos de trabajo actuales |
| staging area | área donde se preparan cambios antes del commit |
| commit | registro de cambios en el repositorio local |
| push | envío de commits al repositorio remoto |
| remote / origin | repositorio externo configurado / nombre habitual del remoto principal |
| main | rama principal del repositorio |
| origin/main | referencia local al estado conocido de `main` en `origin` |
| branch | rama o línea independiente de desarrollo |
| switch | cambiar de rama |
| switch -c | crear una rama y cambiar a ella |
| merge | integrar los commits de una rama en otra |
| upstream / `-u` | vínculo entre la rama local y su rama remota correspondiente |

---

## Sesión — 22 de septiembre de 2026

### Validación de datos del producto

**Términos en inglés:** validation, business rule, whitespace, Bad Request, error code, fail fast.

**Explicación sencilla:** Antes de crear un producto, la API revisa sus datos en un orden definido. `Nombre` y `SKU` no pueden ser `null`, vacíos ni estar compuestos únicamente por espacios. `Precio` no puede ser menor que cero, pero `0` sí es válido.

**Cómo lo razoné:** Para el negocio, un valor como `"   "` no identifica un producto aunque técnicamente tenga caracteres. También decidí permitir `0` porque en el futuro puede representar una promoción como 2 x 1 o 3 x 2.

**Ejemplo:**

~~~csharp
if (string.IsNullOrWhiteSpace(request.Nombre))
{
    return Results.BadRequest(new
    {
        Exito = false,
        Codigo = "INVALID_PRODUCT_NAME",
        Mensaje = "El nombre del producto no puede estar vacío."
    });
}

if (request.Precio < 0)
{
    return Results.BadRequest(new
    {
        Exito = false,
        Codigo = "INVALID_PRODUCT_PRICE",
        Mensaje = "El precio del producto no puede ser menor que cero."
    });
}
~~~

**Estrategia de errores:** La API usa un objeto anónimo por ahora, con `Exito`, `Codigo` y `Mensaje`. Detiene la validación en el primer error y responde con HTTP `400 Bad Request`. La futura clase reutilizable de errores queda pendiente hasta que la repetición la justifique.

**Error o duda:** Al inicio se consideró devolver todos los errores, pero se decidió que para esta etapa una respuesta con el primer error es más clara y evita sobrecargarla.

**Qué aprendí:** Una regla de negocio se convierte en una condición concreta del código. `string.IsNullOrWhiteSpace` cubre `null`, `""` y espacios; `request.Precio < 0` permite exactamente el valor `0`.

---
# Bitácora de aprendizaje — 2026-09-23

## Objetivo de la sesión

Comprender e implementar un `ErrorHandlingMiddleware` en ASP.NET Core y entender cómo participa en el HTTP Request Pipeline.

## Experiencia de aprendizaje

Al inicio, `Middleware`, `Pipeline`, `RequestDelegate`, `_next` y `HttpContext` se sentían abstractos y era difícil separar el middleware de las herramientas usadas para construirlo. La confusión principal era imaginar que el middleware debía revisar directamente si la request tenía `Nombre`, `Sku` o `Precio` y decidir si podía pasar.

La idea se aclaró al separar responsabilidades:

- `HttpContext` representa el contexto completo de la request actual, incluyendo `Request` y `Response`.
- `CrearProductoRequest` representa los datos que necesita la operación concreta de crear un producto.
- El middleware no necesita conocer las propiedades del producto. Su responsabilidad es dejar continuar la request y manejar una excepción que llegue hasta él.

La analogía que ayudó fue pensar en el middleware como un vigilante dentro del edificio: ya estaba contratado, pero no hacía nada hasta que lo registramos en el edificio, es decir, en el pipeline.

## Conceptos aprendidos

### Middleware y pipeline

Un middleware es una pieza del HTTP Request Pipeline que ejecuta una responsabilidad sobre la request y/o la response. Puede dejar continuar el procesamiento mediante `_next(context)` o intervenir y generar una respuesta.

El pipeline se entendió como el recorrido de una request desde que llega hasta que se produce una response:

```text
HTTP Request
    ↓
ErrorHandlingMiddleware
    ↓ _next(context)
Resto del pipeline / endpoint
    ↓
HTTP Response
```

### Manejo de errores

El `try/catch` se colocó alrededor de `await _next(context)`. Si ocurre una excepción durante el resto del procesamiento, el `catch` puede preparar una respuesta controlada:

```text
excepción
    ↓
catch
    ↓
StatusCode = 400
ContentType = application/json
    ↓
ErrorResponse
    ↓
JSON en Response.Body
```

También quedó claro que convertir cualquier excepción en `400 Bad Request` es una simplificación del ejercicio. Un problema de base de datos podría corresponder a `500`, mientras que autenticación, autorización, recursos inexistentes y conflictos tienen otros significados. Refinar esa clasificación queda pendiente.

## Error encontrado y resolución

El middleware estaba construido, pero no funcionaba porque no se había registrado en `Program.cs`. Se agregó:

```csharp
app.UseMiddleware<ErrorHandlingMiddleware>();
```

La solución permitió que la clase formara parte realmente del pipeline.

## Resultado tangible

Se implementó y probó el flujo básico de `ErrorHandlingMiddleware` para el caso de una request de creación de producto con datos faltantes, como un `Sku` ausente:

1. ASP.NET Core intenta procesar la request.
2. Ocurre una excepción.
3. El middleware la captura.
4. Construye un `ErrorResponse` con `Exito`, `Codigo` y `Mensaje`.
5. Serializa el objeto a JSON.
6. Escribe el JSON en `Response.Body` y lo devuelve al cliente, probado desde Postman.

## Qué queda pendiente

- Diferenciar las excepciones y asignar códigos HTTP adecuados en lugar de usar `400` para cualquier excepción.
- Mantener el middleware general, sin acoplarlo a `CrearProductoRequest`.
- Continuar validando el vertical de Product Management con las pruebas y validaciones previstas para el MVP.

## Reflexión

La comprensión no apareció al memorizar la definición de middleware, sino al conectar el flujo completo: request, `_next(context)`, excepción, `catch`, `ErrorResponse`, serialización y response. La distinción entre el contexto HTTP completo y los datos específicos de una operación fue el aprendizaje más importante de la sesión.

---

## Sesión — 24 de septiembre de 2026

### Git: ramas, historial y sincronización

**Términos en inglés:** working directory, staging area, commit, push, origin, main, branch, switch, merge, upstream.

**Explicación sencilla:** El working directory es donde están los archivos que estoy modificando. `git add` lleva cambios al staging area; `git commit` los registra en el repositorio local; `git push` envía commits al repositorio remoto. `origin/main` representa la referencia local a la rama `main` del remoto llamado `origin`.

**Cómo lo razoné:** La comparación que me ayudó fue pensar en un árbol: el repositorio es el historial, una branch es una rama o línea de desarrollo, un commit es un punto nuevo en esa línea y un merge integra una rama en otra.

**Ejemplo:**

~~~powershell
git status
git add docs/learning-notebook.md
git commit -m "docs: update learning notes"
git push -u origin main
git switch main
git switch -c feature/nueva-funcionalidad
git merge feature/nueva-funcionalidad
git branch -d feature/nueva-funcionalidad
~~~

**Error o duda:** Al principio confundí `git add` con guardar el cambio en el repositorio. También estaba entendiendo por qué una rama nueva puede salir de la rama actual y cómo se relacionan `main`, `origin/main` y el remoto.

**Qué aprendí:** `git add` prepara, `commit` registra localmente y `push` sincroniza con GitHub. `git switch` cambia de rama; `git switch -c` crea una rama y cambia a ella. `merge` integra el trabajo de una rama. `branch -d` elimina una rama local ya integrada. `-u` o `--set-upstream` vincula la rama local con su rama remota para simplificar futuros `push` y `pull`.

**Idea principal:** No necesitamos crear todas las ramas desde el inicio. Se crean cuando existe una necesidad real de separar una funcionalidad, una corrección o un experimento.

---

# Sesión - 28 de septiembre de 2026

## Tema

Modelado de base de datos para ProGear MX.

## Objetivo

Pasar de las reglas de negocio previamente definidas a un modelo conceptual y posteriormente a un ERD utilizando DBDiagram.

---

## Decisiones tomadas

### Producto

Se definió como entidad:

- Id
- Nombre
- Marca
- SKU

La marca forma parte del producto porque dos productos pueden tener el mismo nombre pero pertenecer a diferentes marcas.

Ejemplo:

- Multímetro Fluke
- Multímetro Klein Tools
- Multímetro Steren

Aunque tengan una función similar, son productos diferentes.

El SKU es único.

---

### Inventario

Se decidió separar Inventario de Producto.

Inventario contiene:

- Id
- IdProducto
- Existencia
- Reservado

No contiene información de precios.

La disponibilidad se calcula:

`Disponible = Existencia - Reservado`

No se almacena `Disponible`.

---

### Producto e Inventario

La relación es:

`Producto 1 : 0..1 Inventario`

Esto significa que un producto puede no tener todavía un registro de inventario o puede tener exactamente uno.

Se distinguió entre:

**Sin registro de inventario:**

El producto existe en el catálogo, pero todavía no se ha registrado existencia física.

**Inventario con existencia 0:**

El producto sí tiene un registro de inventario, pero actualmente no quedan unidades.

Por lo tanto:

`ausencia de registro != existencia = 0`

---

### Historial de precios

Se creó la entidad `HistorialPrecio`:

- Id
- IdProducto
- Precio
- FechaInicio
- FechaFin

Relación:

`Producto 1 : 0..N HistorialPrecio`

Un producto puede no tener precio todavía, tener uno o tener múltiples precios históricos.

Ejemplo:

- $1,200
- $1,299
- $1,400

`FechaFin = NULL` representa el precio actualmente vigente.

---

## Cardinalidades estudiadas

### 0..1

Cero o uno.

En ProGear MX:

`Producto 1 : 0..1 Inventario`

Un producto puede tener cero o un registro de inventario.

### 0..N

Cero o muchos.

En ProGear MX:

`Producto 1 : 0..N HistorialPrecio`

Un producto puede tener cero, uno o múltiples registros históricos.

---

## ERD

El modelo fue trasladado a DBML y visualizado en DBDiagram.

Archivos relacionados:

- `progear-mx.dbml`
- `progear-mx-erd.png`

---

## Principios aplicados

- No mezclar responsabilidades entre entidades.
- No almacenar datos derivados innecesariamente.
- No utilizar `0` para representar ausencia de información.
- Mantener trazabilidad histórica cuando el negocio lo requiere.
- Diseñar primero a partir de reglas del negocio.
- Mantener el modelo suficientemente simple para el MVP.

---

# Sesión — 29 de septiembre de 2026

## Tema y objetivo

SQL Server aplicado al modelo de productos, inventario e historial de precios. El objetivo fue convertir la regla “un producto solo puede tener un precio vigente” en una estructura protegida por la base de datos y en un flujo seguro para cambiar el precio.

## Lo que construimos

Partimos de una decisión del dominio: `FechaFin = NULL` representa el precio vigente. A partir de ahí:

1. `HistorialPrecio` conserva cada período de precio en vez de sobrescribir el anterior.
2. El precio anterior se cierra asignando `FechaFin`.
3. El nuevo precio se inserta con `FechaFin = NULL`.
4. Un índice único filtrado impide dos filas vigentes para el mismo producto.
5. `UPDATE` e `INSERT` se ejecutan dentro de una transacción.

También repasamos el papel de las restricciones: `PRIMARY KEY` identifica filas, `FOREIGN KEY` mantiene las relaciones, `NOT NULL` exige datos, `UNIQUE` evita duplicados, `CHECK` protege condiciones y `DEFAULT` puede completar valores automáticamente.

## Duda y razonamiento sobre `@MomentoCambio`

La duda fue si `DECLARE @MomentoCambio` guardaba el “cambio” de estado. La precisión importante fue que guarda **el instante exacto** del cambio. `DECLARE` crea una variable T-SQL, `DATETIME2` indica que almacenará fecha y hora, y `SYSDATETIME()` obtiene el momento actual del servidor SQL.

La variable se captura una sola vez para reutilizar exactamente el mismo valor:

~~~text
@MomentoCambio
      ├── FechaFin del precio anterior
      └── FechaInicio del precio nuevo
~~~

Así no dependemos de dos llamadas distintas a `SYSDATETIME()` que podrían producir instantes ligeramente diferentes.

## Flujo trabajado

~~~sql
BEGIN TRANSACTION;

DECLARE @MomentoCambio DATETIME2 = SYSDATETIME();

UPDATE HistorialPrecio
SET FechaFin = @MomentoCambio
WHERE IdProducto = 1
  AND FechaFin IS NULL;

INSERT INTO HistorialPrecio (IdProducto, Precio, FechaInicio, FechaFin)
VALUES (1, 1600.00, @MomentoCambio, NULL);

COMMIT TRANSACTION;
~~~

`BEGIN TRANSACTION` inicia la operación atómica. `COMMIT` confirma los cambios si todo salió bien. `ROLLBACK` sería la salida si una parte fallara. El resultado observado fue un registro de `$1500` cerrado y un registro de `$1600` vigente. También se observó que puede haber varios registros históricos con el mismo precio; lo relevante es que solo uno tenga `FechaFin = NULL`.

## Qué aprendí

- Una regla de negocio puede traducirse en una restricción y en un flujo de escritura.
- `NULL` puede tener significado de dominio: “vigente actualmente”.
- El historial conserva trazabilidad; cambiar una fila no sustituye registrar el nuevo período.
- Una transacción protege el cambio compuesto de cerrar y abrir precio.
- La base de datos puede ayudar a impedir estados inválidos, no solo almacenar datos.

## Evidencia y límite

La conversación de la sesión reporta que el flujo se ejecutó y se observó en SQL Server. En el checkout actual, la API todavía usa una `List<Producto>` en memoria y el modelo persistente está representado por DBML; aún no hay esquema SQL versionado, `DbContext` ni prueba automatizada desde la API.

## Cierre

Construimos → probamos → reflexionamos → documentamos → cerramos.
---

## Sesión — 6 de octubre de 2026

### Tema y objetivo

Cerrar `HistorialPrecio` comprobando que las reglas definidas antes del
desarrollo se cumplen en los cuatro endpoints trabajados y dejar preparado el
siguiente bloque: `Inventario`.

### Endpoints y reglas comprobadas

- `GET /productos/{id}/precios/historial`: devuelve el historial completo; un
  historial vacío se representa como `[]`.
- `GET /productos/{id}/precio`: devuelve el precio con `FechaFin = NULL` o
  `null` si el producto existe sin precio vigente.
- `GET /productos/{id}/precios/{precioId}`: solo devuelve el registro cuando
  pertenece al producto solicitado.
- `POST /productos/{id}/precios`: rechaza identificadores inválidos, precios
  menores o iguales que cero y la repetición del precio vigente; registra el
  primer precio o cierra el vigente y crea el nuevo.

### Transacción, Commit y Rollback

**Términos en inglés:** transaction, atomicity, Commit, Rollback, scope.

**Explicación sencilla:** Una transacción agrupa el cierre del precio anterior
y la creación del nuevo. La atomicidad significa que el cambio completo se
confirma o se revierte completo. `Commit` confirma; `Rollback` deshace los
cambios no confirmados. El alcance (`scope`) ayuda a entender dentro de qué
operación y contexto es válida una variable o regla.

**Cómo lo razoné:** No bastaba con observar que existían llamadas a commit y
rollback. Se provocó una excepción después de `SaveChangesAsync()` y antes de
confirmar, para comprobar el estado real de la base de datos.

### Prueba controlada

Partiendo de `$2,100` vigente, se intentó registrar `$2,500`. La excepción
controlada pasó por `RollbackAsync()` y el middleware devolvió `500 Internal
Server Error`. Al consultar de nuevo, `$2,100` continuó vigente y `$2,500` no
quedó confirmado. Después se retiró la condición artificial y una prueba normal
posterior funcionó con `201 Created`.

### Pruebas registradas

Se conservaron las pruebas manuales `PEG-001` a `PEG-009` y `PEP-010` a
`PEP-015`. `PEG` significa **Prueba Endpoint GET** y `PEP` significa **Prueba
Endpoint POST**. La matriz con sus endpoints y resultados está en
`docs/requirements/requirements.md`.

### Conceptos aprendidos o confirmados

- `DbSet` representa una entidad en el `DbContext` y permite consultar o seguir
  sus registros.
- `Commit` confirma una transacción y `Rollback` revierte sus cambios.
- La prueba controlada sirve para validar una propiedad de integridad bajo una
  falla provocada, sin confundirla con el flujo normal.
- La evidencia de pruebas manuales permite cerrar esta funcionalidad, mientras
  que las pruebas automatizadas quedan como mejora futura.

### Cierre y siguiente bloque

`HistorialPrecio` queda cerrado conforme a las reglas de negocio establecidas.
El siguiente bloque de desarrollo es `Inventario`.

 Construimos → probamos → reflexionamos → documentamos → cerramos.
---

## Sesión — 1 de octubre de 2026

### Tema y objetivo

Conectar el catálogo de productos de ProGear MX a SQL Server mediante Entity
Framework Core y comprobar que la API, la base de datos y las herramientas de
consulta muestran el mismo estado.

### Lo que construimos

- `ProGearDbContext` quedó configurado para trabajar con `Producto`,
  `Inventario` e `HistorialPrecio`.
- `OnModelCreating` mapea tablas, claves, relaciones, longitudes, obligatoriedad,
  unicidad y precisión decimal.
- `GET /productos` consulta SQL Server con `ToListAsync()`.
- `GET /productos/{id}` busca por clave primaria con `FindAsync(id)`.
- `POST /productos` recibe `CrearProductoRequest`, valida nombre, SKU y SKU
  duplicado con `AnyAsync()`, agrega el producto con `Add()` y persiste con
  `SaveChangesAsync()`.
- Se eliminó la lista local de productos y `siguienteId`.
- SQL Server genera el identificador mediante `IDENTITY`.

### Pruebas y experiencia

Se verificó una creación válida y la generación de los identificadores 6 y 7.
También se probaron un SKU duplicado, un nombre vacío, un SKU vacío y una
request incompleta. Los resultados coincidieron al revisar la API desde
Postman, el navegador y consultas directas a SQL Server.

### Conceptos aprendidos o confirmados

**Términos en inglés:** `async`, `await`, `try/catch`, `Add`,
`SaveChangesAsync`, Entity Framework Core, `DbContext`, `ToListAsync`,
`FindAsync`, `AnyAsync`, `IDENTITY`.

**Explicación sencilla:** `async` permite trabajar con operaciones asíncronas y
`await` espera su resultado. `try/catch` permite manejar excepciones. EF Core
traduce expresiones de C# a SQL; `DbContext` funciona como contexto y mapa entre
los modelos y la base de datos. `Add` deja una entidad en seguimiento y
`SaveChangesAsync` persiste los cambios. SQL Server genera el `Id` porque la
columna está configurada con `IDENTITY`.

**Cómo lo razoné:** La diferencia principal frente a la etapa anterior fue
comprobar que la API ya no busca en una lista limitada en memoria. Al consultar
con `FindAsync(id)`, el endpoint apunta a la tabla de SQL Server y puede
encontrar registros que no estaban en aquella lista. Después confirmé que la
creación también llega a la base de datos y que el identificador no se asigna
manualmente.

**Duda o error que ayudó a aprender:** El problema del sexto producto se
explicó porque el endpoint todavía buscaba en una lista que solo tenía cinco
elementos. Cambiarlo a `FindAsync(id)` alineó la consulta por identificador con
el GET general, que ya consultaba SQL Server.

### Límite de la sesión

Aunque `Inventario` e `HistorialPrecio` ya tienen modelos y mapeos en
`ProGearDbContext`, no se avanzó en implementar sus operaciones. Ese trabajo
queda pendiente.

### Cierre

Construimos → probamos → reflexionamos → documentamos → cerramos.

---

## Sesión — 2 de octubre de 2026

### Tema y objetivo

Cerrar el trabajo de consulta de `HistorialPrecio` mediante dos endpoints y
entender qué está haciendo EF Core en cada consulta.

### Mapeo de `HistorialPrecio` en EF Core

**Términos en inglés:** `HasKey`, `HasPrecision`, `HasOne`, `WithMany`,
`HasForeignKey`, `DbSet`, relationship, cardinality.

**Explicación sencilla:** `HistorialPrecio` se registra en el
`ProGearDbContext` como `DbSet`. En `OnModelCreating`, `HasKey(h => h.Id)`
indica su clave primaria; `HasPrecision(10, 2)` configura la precisión del
precio; y `HasOne<Producto>().WithMany().HasForeignKey(h => h.IdProducto)`
expresa que cada registro pertenece a un producto y que un producto puede
tener cero, uno o muchos registros históricos.

**Cómo lo razoné:** La relación no significa que todo producto deba tener un
precio. Significa que un producto puede existir sin registros, pero cada
registro de `HistorialPrecio` necesita apuntar a un `Producto`: `Producto 1 :
0..N HistorialPrecio`.

### Consultas y traducción conceptual

`Where(h => h.IdProducto == id).ToListAsync()` filtra el historial por
producto y ejecuta la consulta de forma asíncrona. Conceptualmente, EF Core
traduce la expresión a una consulta parecida a:

~~~sql
SELECT *
FROM HistorialPrecio
WHERE IdProducto = @id;
~~~

Para el precio vigente se usó `Where(h => h.IdProducto == id &&
h.FechaFin == null).SingleOrDefaultAsync()`. `SingleOrDefaultAsync()` expresa
que se espera cero o un registro: devuelve `null` si no hay ninguno, devuelve
el único registro si existe uno y falla si hay más de uno. La regla de un solo
precio vigente hace que ese último caso represente un estado inconsistente.

### Colección frente a recurso único

El historial completo es una colección: sin registros llega como `[]`, porque
la respuesta representa una lista aunque esté vacía. El precio vigente es un
recurso único: si el producto existe pero no tiene precio vigente, llega como
`null`. El `1` que se veía a la izquierda en Postman era el número de línea del
editor de respuesta, no el contenido de la respuesta. Compararlo con la línea
inicial de un archivo vacío en VS Code ayudó a interpretar correctamente la
evidencia.

### Endpoints y pruebas realizadas

- `GET /productos/{id}/precios/historial`: valida un identificador mayor que
  cero, devuelve `400` para un identificador inválido, `404` si el producto no
  existe y `200` con una colección, incluso cuando está vacía.
- `GET /productos/{id}/precio`: valida un identificador mayor que cero,
  devuelve `400` para un identificador inválido, `404` si el producto no
  existe y `200` con el registro vigente o `null` cuando todavía no existe.

Se probaron las respuestas para un producto con historial y precio vigente,
para un producto sin precio vigente, para un identificador inválido y para un
producto inexistente. También se comparó la respuesta observada en Postman con
la consulta y el código del endpoint.

### Qué aprendí

- El mapeo de EF Core conecta propiedades C# con claves, precisión y
  relaciones de la base de datos.
- `ToListAsync()` representa una consulta de colección; `SingleOrDefaultAsync()`
  representa una consulta de recurso único con semántica 0/1/más de 1.
- `[]` y `null` comunican situaciones distintas: colección vacía frente a
  recurso único ausente.
- Debugging es revisar la evidencia completa y cuestionar su interpretación,
  no cambiar el endpoint a ciegas.

### Límite de la sesión

Se completaron las consultas GET de `HistorialPrecio`, pero no se implementó
el cambio de precio desde la API, ni transacciones nuevas, ni operaciones de
`Inventario`.

### Cierre

Construimos → probamos → reflexionamos → documentamos → cerramos.

---

# Sesión - 8 de octubre de 2026

### Tema y objetivo

Diseñar y comenzar la implementación del módulo de `Inventario`, definir sus
reglas de negocio y construir los primeros endpoints para consultar y registrar
existencias.

El objetivo fue entender cómo relacionar `Producto` e `Inventario` desde EF
Core, diferenciar DTOs de entrada y salida, y aplicar las reglas de inventario
mediante validaciones y decisiones de flujo.

### Modelo y reglas de `Inventario`

**Términos en inglés:** `JOIN`, `INNER JOIN`, `Request DTO`, `Response DTO`,
`business rule`, `flow`.

**Explicación sencilla:** `Producto` puede existir sin tener todavía un registro
de `Inventario`. La ausencia del registro significa que todavía no se ha
establecido inventario para ese producto. Una vez que existe el registro, puede
tener `Existencia = 0`, lo que representa un inventario registrado pero sin
stock.

Las reglas establecidas fueron:

- `Existencia >= 0`.
- `Reservado >= 0`.
- `Reservado <= Existencia`.
- `Disponible = Existencia - Reservado`.
- `Disponible >= 0`.

Las operaciones de dominio definidas son entrada, reserva, salida o
confirmación de venta, y liberación o cancelación. En esta sesión solo se
implementaron la consulta general y la entrada de mercancía.

### Relación entre `Producto` e `Inventario`

**Términos en inglés:** `foreign key`, `primary key`, `relationship`,
`cardinality`.

La relación es:

```text
Producto 1 : 0..1 Inventario
```

`Inventario.IdProducto` referencia a `Producto.Id`. Un producto puede no tener
fila en `Inventario`, pero no debe tener más de una. Esta diferencia conserva
dos significados distintos:

```text
Producto existe + no existe Inventario = inventario aún no establecido
Producto existe + existe Inventario + Existencia = 0 = inventario registrado sin stock
```

### `InventarioResponse`

Se creó `InventarioResponse` como `Response DTO`. No representa una tabla;
combina información de `Producto`, `Inventario` y el valor calculado
`Disponible`.

```text
IdProducto, Nombre, Marca, Sku,
Existencia, Reservado, Disponible
```

`Disponible` no se almacena directamente:

```text
Disponible = Existencia - Reservado
```

### `CantidadRequest`

Se creó `CantidadRequest` como `Request DTO` reutilizable para las operaciones
que reciben una cantidad.

```csharp
public class CantidadRequest
{
    public required int Cantidad { get; set; }
}
```

La misma estructura puede servir para entradas, reservas, salidas y
liberaciones cuando esas operaciones se implementen.

### `GET /inventario`

Se implementó el endpoint de consulta general mediante un `Join` entre
`Inventario` y `Producto`:

```text
Inventario.IdProducto == Producto.Id
        ↓
combinar datos de ambas entidades
        ↓
calcular Disponible
        ↓
devolver la colección
```

El `Join` tiene semántica de `INNER JOIN`: solo aparecen registros con
correspondencia en ambas entidades. Por eso un producto sin inventario no
aparece en `GET /inventario`, aunque un inventario existente con `Existencia = 0`
sí puede aparecer.

### `POST /inventario/{idProducto}/entradas`

Se implementó la primera operación de modificación: registrar una entrada de
mercancía.

El flujo es:

```text
Recibir idProducto y CantidadRequest
        ↓
Validar idProducto
        ↓
Buscar producto
        ↓
Validar Cantidad
        ↓
Buscar Inventario
        ↓
Crearlo o incrementar Existencia
        ↓
Guardar cambios
        ↓
Construir InventarioResponse
        ↓
201 Created
```

Si no existe inventario, se crea con `Existencia = Cantidad` y `Reservado = 0`.
Si ya existe, se incrementa `Existencia`. `Reservado` no cambia con una entrada
y `Disponible` se vuelve a calcular.

### `ErrorResponse` y validaciones

El endpoint reutiliza `ErrorResponse`, compuesto por `Exito`, `Codigo` y
`Mensaje`.

- `400 Bad Request`, `INVALID_PRODUCT_ID`, si `idProducto <= 0`.
- `404 Not Found`, `PRODUCT_NOT_FOUND`, si el producto no existe.
- `400 Bad Request`, `INVALID_INVENTORY_QUANTITY`, si `Cantidad <= 0`.

La ausencia de inventario no es un error en esta operación: representa el caso
válido de la primera entrada y provoca la creación del registro.

### Pruebas realizadas

Las pruebas documentadas fueron manuales y corresponden a la entrada de
mercancía:

- `PEI-001`: primera entrada de 10 unidades → `201 Created`; `Existencia = 10`,
  `Reservado = 0`, `Disponible = 10`.
- `PEI-002`: entrada adicional de 5 unidades → `201 Created`; `Existencia = 15`
  sin crear otro registro.
- `PEI-003`: cantidad `0` → `400 Bad Request`,
  `INVALID_INVENTORY_QUANTITY`.
- `PEI-004`: cantidad `-5` → `400 Bad Request`,
  `INVALID_INVENTORY_QUANTITY`.
- `PEI-005`: `idProducto = -1` → `400 Bad Request`, `INVALID_PRODUCT_ID`.
- `PEI-006`: `idProducto = 0` → `400 Bad Request`, `INVALID_PRODUCT_ID`.
- `PEI-007`: producto inexistente → `404 Not Found`, `PRODUCT_NOT_FOUND`.
- `PEI-008`: entrada con una reserva existente → pendiente hasta implementar
  el endpoint de reservas.

### Cómo lo razoné

El endpoint se entendió como una secuencia de decisiones, no solo como código:

```text
¿ID válido?
    ↓
¿Producto existe?
    ↓
¿Cantidad válida?
    ↓
¿Existe inventario?
    ├── no → crear
    └── sí → incrementar Existencia
    ↓
Guardar y responder
```

Primero se define qué debe suceder, qué condiciones permiten continuar, qué
estado cambia y qué respuesta recibe el consumidor. Después ese razonamiento se
traduce a C# y EF Core.

### Qué aprendí

- `JOIN` combina información relacionada; `INNER JOIN` conserva solo las
  correspondencias.
- Un `Request DTO` y un `Response DTO` tienen responsabilidades diferentes.
- `InventarioResponse` puede contener datos de varias entidades y un valor
  calculado que no existe como columna.
- Una entrada crea el inventario si no existe o incrementa `Existencia` si ya
  existe.
- `Reservado` no cambia al recibir mercancía.
- Las validaciones forman parte de las reglas del dominio.
- El pseudocódigo ayuda a ordenar el flujo antes de escribir sintaxis.

### Límite de la sesión

Se completaron el modelo y sus reglas, `InventarioResponse`, `CantidadRequest`,
`GET /inventario`, `POST /inventario/{idProducto}/entradas` y las pruebas
`PEI-001` a `PEI-007`.

Quedan pendientes:

- `GET /inventario/{idProducto}`.
- `POST /inventario/{idProducto}/reservas`.
- `POST /inventario/{idProducto}/salidas`.
- `POST /inventario/{idProducto}/liberaciones`.
- `PEI-008`, que depende de reservas.

### Cierre

Construimos → probamos → reflexionamos → documentamos → cerramos.
