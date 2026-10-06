# GRUPO 5 - Trabajo Practico 1 

 ### Preguntas Conceptuales ###

 **¿Puedes identificar pruebas de unidad y de integración en la práctica que se realizó?**
 **Unitarias**: Pruebas directas sobre Producto.ActualizarPrecio y la prueba de AplicarDescuento donde se aísla Producto usando un mock (Moq).
 Se instancia únicamente Producto, se manipula su método interno y se valida su estado o su excepción de dominio sin tocar Tienda ni colecciones externas.

 **Integración**: Todas las pruebas donde Tienda opera con instancias concretas de Producto, alcanzando su máxima expresión en la suite CarritoTests, donde se comprueba el flujo integral de inventario, descuentos y liquidación final.
 Verifican el caso de uso completo (integración basada en hebras/flujo):

**El fixture** carga varios objetos Producto en el inventario de Tienda.

Se aplica una modificación de precios o descuentos (Tienda actualizando Producto).

Se arma una lista de compras (carrito).

**CalcularTotalCarrito** recorre la colección, invoca **BuscarProducto**, extrae los precios actualizados de cada entidad y acumula el total.
Valida que todas las piezas (Tienda, Producto, Inventario y la lógica de carrito) colaboren correctamente de extremo a extremo.

### Podría haber escrito las pruebas primero antes de modificar el código de la aplicación?
### ¿Cómo sería el proceso de escribir primero los tests? Describe el proceso con mis palabras.
Sí, totalmente, de hecho, esa es la forma ideal de trabajar en desarrollo de software moderno y se conoce como TDD (Test-Driven Development o Desarrollo Guiado por Pruebas).
Si hubiéramos aplicado este enfoque antes de tocar las clases Producto o Tienda, el proceso habría seguido el ciclo clásico iterativo conocido como Rojo - Verde - Refactor:

##### 1. Fase Roja (Escribir la prueba que falla)
Antes de escribir una sola línea de lógica en la aplicación, nos sentamos a pensar qué comportamiento esperamos que tenga el sistema y escribimos el test primero.
##### 2. Fase Verde (Escribir el código mínimo indispensable)
Una vez que el test falla, vamos al código de nuestra aplicación (Producto.cs) y escribimos únicamente la cantidad mínima de código necesaria para que la prueba pase a Verde.
##### 3.Fase Refactor (Limpiar y optimizar sin miedo)
Ahora que tenemos una "red de seguridad" automatizada que nos avisa si algo se rompe, podemos mejorar el código.

Qué se hace: Revisamos el código para eliminar duplicaciones, ordenar nombres de variables o mejorar la estructura interna.
La ventaja es que cada cambio que hacemos lo comprobamos volviendo a correr los tests en segundos; si siguen en verde, tenemos la certeza absoluta de que no introdujimos ninguna regresión (errores colaterales).

##### En lo que va del trabajo práctico, ¿puedes identificar 'Controladores' y 'Resguardos'? ¿Qué es un “test double”? ¿Hay otros nombres para los objetos/funciones simulados?

**Los Controladores (Drivers)**
Nuestras clases y métodos de prueba de xUnit (TiendaTests): En un entorno automatizado con frameworks, cada método de prueba anotado con [Fact] actúa como un Driver.

Por ejemplo, en ActualizarPrecio_PrecioValido_CambiaElPrecio, el método de prueba asume el rol del programa principal: instancia a Producto, invoca producto.ActualizarPrecio(1500) pasándole argumentos y captura la salida para compararla en el Assert.

También el menú interactivo por consola (Program.cs) actuaba como un controlador manual para disparar las operaciones de Tienda.

**Los Resguardos (Stubs):**

En las primeras etapas (1 y 2), no se usaron resguardos porque Tienda se integraba directamente con instancias concretas y reales de Producto.

El concepto de resguardo aparece formalmente en la Etapa 3 (Uso de dobles para aislar unidades) con la prueba:En **productoMock** ocupa el rol de resguardo / objeto simulado: se sitúa por debajo de Tienda para que esta pueda invocar **ActualizarPrecio** sobre él sin depender de la lógica real ni alterar el estado de un producto genuino.

**Un doble de prueba (test double)** es un término genérico que agrupa a cualquier objeto o componente que reemplaza a una dependencia real en un entorno de pruebas con el propósito de aislar la unidad bajo prueba, ganar velocidad, evitar efectos colaterales indeseados o simplificar la configuración.
Otros nombres: Dummy (Maniquí), Stub (Resguardo / Cabo), Mock (Simulador con verificación de interacción), Fake (Falso / Simulación ligera)

##### Defina usando palabras propias y según la práctica realizada qué es un fixture.¿Qué ventajas ve en el uso de fixtures? ¿Qué enfoque de diseño de pruebas estaríamos aplicando (caja negra/blanca)? Explique los conceptos de Setup y Teardown en testing.

Un fixture es un mecanismo provisto por los frameworks de prueba que permite preparar y dejar listo un entorno o estado inicial conocido y controlado antes de que se ejecuten los tests, y limpiarlo al finalizar.
En nuestra práctica de C# con xUnit, el fixture fue la clase **TiendaFixture**. En lugar de tener que instanciar manualmente en cada método de prueba una nueva Tienda y cargarle uno por uno los productos ("Pan", "Leche", "Queso", "Yerba", "Café"), centralizamos esa configuración previa en un único lugar compartido. Así, cada test arranca asumiendo que ese catálogo de productos ya existe en el inventario.
**Ventajas del uso de fixtures**
Eliminación de código duplicado (principio DRY): Evita repetir las mismas 10 o 15 líneas de inicialización (el bloque Arrange) en cada uno de los métodos de prueba.

Mantenibilidad: Si el constructor de Producto o de Tienda cambia (por ejemplo, si agregamos un nuevo campo obligatorio como stock), solo modificamos el constructor de TiendaFixture y no decenas de pruebas individuales.

Consistencia y legibilidad: Todas las pruebas operan sobre un conjunto estandarizado de datos conocidos, haciendo que los tests sean más cortos, claros y enfocados directamente en la acción que quieren evaluar (Act) y su resultado esperado (Assert).

Aislamiento controlado del estado:En frameworks modernos permite definir el ciclo de vida del estado (por ejemplo, una instancia limpia por clase de prueba con IClassFixture o por método de prueba) para evitar que efectos secundarios de un test rompan a los demás.
##### ¿Qué enfoque de diseño de pruebas estaríamos aplicando (caja negra/blanca)? 
En la práctica realizada estuvimos aplicando predominantemente un enfoque de Caja Negra (Funcional / Basado en Entradas y Salidas), complementado con matices de Caja Blanca en las pruebas de excepciones:

**Predominio de Caja Negra:**
Diseñamos los casos pensando en la especificación del requerimiento y el contrato público de las clases. Por ejemplo:

Le pasamos una lista con ["Pan", "Leche"] y comprobamos que devuelva $2900.

Aplicamos un 10% de descuento a "Yerba" y comprobamos que el total baje a $720.
Evaluamos qué hace el sistema ante ciertas entradas sin depender de cómo están estructurados los bucles foreach o los métodos internos de Tienda.

**Matices de Caja Blanca (Pruebas Estructurales):**
Al diseñar las pruebas de excepciones (ActualizarPrecio con valor negativo o buscar un producto inexistente), nos aseguramos de ejercitar las ramas lógicas específicas (if (nuevoPrecio < 0) o if (producto is null)), lo que responde al criterio de cobertura de ramas/decisiones propio de caja blanca.

##### Explique los conceptos de Setup y Teardown en testing.
Forman parte del ciclo de vida estándar de cualquier batería de pruebas automatizadas:
**Setup (Preparación / Configuración)**
Es el bloque o método que se ejecuta antes de correr la prueba.

**Teardown (Limpieza / Desmantelamiento)**
Es el bloque o método que se ejecuta después de que la prueba finaliza (sin importar si la prueba pasó o falló).

##### ¿Realizó una prueba de cobertura completa? ¿Qué tipo de cobertura utilizó?
No se realizó una prueba de cobertura completa (en todos los caminos y combinaciones), aunque sí se alcanzó un nivel muy alto de cobertura de sentencias y ramas en los métodos principales.
##### Cobertura de Sentencias (Statement Coverage):

Casi la totalidad de las líneas ejecutables de Producto y Tienda fueron transitadas al menos una vez durante las pruebas.

Por ejemplo, en Producto.ActualizarPrecio, la línea Precio = nuevoPrecio; y la línea throw new ManejarExcepciones(...) se ejecutan en sus respectivos tests.

##### Cobertura de Ramas / Decisiones (Branch Coverage):

Se evaluaron los dos caminos posibles (true y false) de las estructuras condicionales críticas:

**En ActualizarPrecio:** la condición nuevoPrecio < 0 se evaluó tanto en verdadero (lanza excepción) como en falso (asigna el precio).

**En BuscarProducto**: la condición producto is null se evaluó cuando el producto existe (retorna la entidad) y cuando no existe (lanza excepción).

**En EliminarProducto:** se evaluó tanto la rama donde el ID existe y se elimina, como donde no existe y lanza excepción.

#### ¿Puede describir una situación de desarrollo para este caso en donde se plantee pruebas deintegración ascendente? Describa la situación.

Capa Inferior (Base de Datos / Entidades): Clase Producto y un repositorio de persistencia ProductoRepository (que guarda y consulta productos en una base de datos o almacenamiento en disco).

Capa Intermedia (Lógica de Negocio): Clase Tienda (administra el stock, las reglas de descuento y validaciones).

Capa Superior (Controlador / Interfaz): Clase CarritoController o la interfaz de usuario por consola / web API que recibe las peticiones del cliente.

##### ¿Cómo se plantearía la integración ascendente en este caso?
La integración ascendente (Bottom-Up) comienza probando los módulos del nivel más bajo e independiente, y va subiendo paso a paso hacia los niveles superiores. No requiere resguardos (stubs), pero sí exige construir controladores (drivers) en cada nivel para estimular a los módulos integrados:

**Paso 1:** Probar la base (Nivel 1)
Se prueba y valida completamente la clase Producto y sus operaciones directas (ActualizarPrecio). Al ser la unidad base independiente, no depende de nadie.

**Paso 2:** Integrar el nivel inferior con el nivel intermedio (Nivel 1 + Nivel 2)
Se integra Tienda con las clases Producto ya verificadas.
Como la interfaz de usuario o el controlador web todavía no existen (o no se quieren involucrar aún), usamos un driver de prueba (nuestra suite de xUnit TiendaTests) que simula al usuario agregando productos, buscando por nombre y aplicando descuentos.

**Paso 3:** Integrar el subsistema de compras (Nivel 2 + Flujo de Carrito)
Una vez que Tienda y Producto funcionan de forma integrada y confiable, se integra la lógica de liquidación CalcularTotalCarrito.
El driver de prueba (en este caso, CarritoTests) envía listas de productos y valida que los totales y descuentos coordinen perfectamente entre el inventario y los productos.

**Paso 4:** Integrar la interfaz superior (Nivel 3)
Por último, se conectan los controladores de entrada (Program.cs o la API REST). Como todos los componentes subordinados ya fueron probados e integrados previamente desde la base, cualquier error que aparezca en este paso pertenecerá casi con certeza a la capa de presentación o captura de datos por consola.
