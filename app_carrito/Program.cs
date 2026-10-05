using Carrito;

var tienda = new Tienda();

while (true)
{
    Console.WriteLine("\n--- Tienda ---");
    Console.WriteLine("1. Agregar producto");
    Console.WriteLine("2. Mostrar productos");
    Console.WriteLine("3. Buscar por nombre");
    Console.WriteLine("4. Eliminar por ID");
    Console.WriteLine("5. Salir");
    Console.Write("Opción: ");

    var opcion = Console.ReadLine();

    switch (opcion)
    {
        case "1":
            Console.Write("Nombre: ");
            var nombre = Console.ReadLine() ?? "";

            Console.Write("Precio: ");
            if (!decimal.TryParse(Console.ReadLine(), out var precio))
            {
                Console.WriteLine("Precio inválido.");
                break;
            }

            Console.Write("Categoría: ");
            var categoria = Console.ReadLine() ?? "";

            tienda.AgregarProducto(new Producto(nombre, precio, categoria));
            Console.WriteLine("Producto agregado.");
            break;

        case "2":
            if (tienda.Inventario.Count == 0)
            {
                Console.WriteLine("El inventario está vacío.");
                break;
            }

            foreach (var p in tienda.Inventario)
            {
                Console.WriteLine($"[{p.IdProducto}] {p.Nombre} - {p.Categoria} - ${p.Precio}");
            }
            break;

        case "3":
            Console.Write("Nombre a buscar: ");
            var buscado = Console.ReadLine() ?? "";

            // Opción 3 - buscar (antes: chequeo de null, ver STAGE_1_REPLICATION.md).
            // Etapa 2: BuscarProducto lanza ManejarExcepciones si no existe.
            try
            {
                // Implementación anterior - Etapa 1:
                // var resultado = tienda.BuscarProducto(buscado);
                // if (resultado is null)
                // {
                //     Console.WriteLine("Producto no encontrado.");
                // }
                // else
                // {
                //     Console.WriteLine($"[{resultado.IdProducto}] {resultado.Nombre} - {resultado.Categoria} - ${resultado.Precio}");
                // }

                var resultado = tienda.BuscarProducto(buscado);
                Console.WriteLine($"[{resultado.IdProducto}] {resultado.Nombre} - {resultado.Categoria} - ${resultado.Precio}");

                // Implementación anterior - Etapa 2 (menú tras buscar):
                // Console.WriteLine("1. Actualizar precio");
                // Console.WriteLine("2. Salir");

                // Etapa 3: se agrega aplicar descuento y se renumera la salida como "Volver".
                Console.WriteLine("1. Actualizar precio");
                Console.WriteLine("2. Aplicar descuento");
                Console.WriteLine("3. Volver");
                Console.Write("Opción: ");
                var opcionProducto = Console.ReadLine();

                if (opcionProducto == "1")
                {
                    Console.Write("Nuevo precio: ");
                    if (!decimal.TryParse(Console.ReadLine(), out var nuevoPrecio))
                    {
                        Console.WriteLine("Precio inválido.");
                    }
                    else
                    {
                        try
                        {
                            resultado.ActualizarPrecio(nuevoPrecio);
                            Console.WriteLine($"Precio actualizado: ${resultado.Precio}");
                        }
                        catch (ManejarExcepciones ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else if (opcionProducto == "2")
                {
                    Console.Write("Porcentaje de descuento: ");
                    if (!decimal.TryParse(Console.ReadLine(), out var porcentaje))
                    {
                        Console.WriteLine("Porcentaje inválido.");
                    }
                    else
                    {
                        try
                        {
                            tienda.AplicarDescuento(resultado.Nombre, porcentaje);
                            Console.WriteLine($"Nuevo precio: ${resultado.Precio}");
                        }
                        catch (ManejarExcepciones ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else if (opcionProducto != "3")
                {
                    Console.WriteLine("Opción no válida.");
                }
            }
            catch (ManejarExcepciones ex)
            {
                Console.WriteLine(ex.Message);
            }
            break;

        case "4":
            Console.Write("ID a eliminar: ");
            if (!int.TryParse(Console.ReadLine(), out var id))
            {
                Console.WriteLine("ID inválido.");
                break;
            }

            // Opción 4 - eliminar (antes: ternario sobre el bool).
            // Etapa 2: EliminarProducto lanza ManejarExcepciones si no existe.
            try
            {
                // Implementación anterior - Etapa 1:
                // Console.WriteLine(tienda.EliminarProducto(id)
                //     ? "Producto eliminado."
                //     : "No existe un producto con ese ID.");

                tienda.EliminarProducto(id);
                Console.WriteLine("Producto eliminado.");
            }
            catch (ManejarExcepciones ex)
            {
                Console.WriteLine(ex.Message);
            }
            break;

        case "5":
            return;

        default:
            Console.WriteLine("Opción no válida.");
            break;
    }
}
