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

            var resultado = tienda.BuscarProducto(buscado);
            if (resultado is null)
            {
                Console.WriteLine("Producto no encontrado.");
            }
            else
            {
                Console.WriteLine($"[{resultado.IdProducto}] {resultado.Nombre} - {resultado.Categoria} - ${resultado.Precio}");
            }
            break;

        case "4":
            Console.Write("ID a eliminar: ");
            if (!int.TryParse(Console.ReadLine(), out var id))
            {
                Console.WriteLine("ID inválido.");
                break;
            }

            Console.WriteLine(tienda.EliminarProducto(id)
                ? "Producto eliminado."
                : "No existe un producto con ese ID.");
            break;

        case "5":
            return;

        default:
            Console.WriteLine("Opción no válida.");
            break;
    }
}
