using Carrito;

namespace UnitTestingxUnit.Tests;

/// Fixture de xUnit: prepara una Tienda con productos predefinidos
/// para reutilizar el estado inicial en las pruebas.
public class TiendaFixture
{
    public Tienda Tienda { get; }
    public Producto Pan { get; }
    public Producto Leche { get; }
    public Producto Queso { get; }

    // Etapa 5: productos agregados como estado inicial de las pruebas del carrito.
    public Producto Yerba { get; }
    public Producto Cafe { get; }

    public TiendaFixture()
    {
        Tienda = new Tienda();

        Pan = new Producto("Pan", 1000, "Almacén");
        Leche = new Producto("Leche", 2000, "Lácteos");
        Queso = new Producto("Queso", 3000, "Lácteos");

        Tienda.AgregarProducto(Pan);
        Tienda.AgregarProducto(Leche);
        Tienda.AgregarProducto(Queso);

        Yerba = new Producto("Yerba", 800, "Almacén");
        Cafe = new Producto("Café", 1200, "Almacén");

        Tienda.AgregarProducto(Yerba);
        Tienda.AgregarProducto(Cafe);
    }
}
