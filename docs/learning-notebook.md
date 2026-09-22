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

---

## Futuras sesiones

- [ ] Crear y revisar productos.json.
- [ ] Usar System.Text.Json para deserializar a List<Producto>.
- [ ] Leer el archivo desde la aplicación.
- [ ] Conectar la lista cargada con un endpoint de productos.
- [ ] Validar las reglas de negocio de nombre, SKU y precio.
- [ ] Registrar nuevos aprendizajes, errores y razonamientos con esta misma estructura.

