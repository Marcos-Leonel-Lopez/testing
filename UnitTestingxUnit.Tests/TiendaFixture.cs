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

    public TiendaFixture()
    {
        Tienda = new Tienda();

        Pan = new Producto("Pan", 1000, "Almacén");
        Leche = new Producto("Leche", 2000, "Lácteos");
        Queso = new Producto("Queso", 3000, "Lácteos");

        Tienda.AgregarProducto(Pan);
        Tienda.AgregarProducto(Leche);
        Tienda.AgregarProducto(Queso);
    }
}
