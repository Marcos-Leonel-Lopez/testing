using Carrito;

namespace UnitTestingxUnit.Tests;

/// Pruebas de las operaciones básicas de Tienda y Producto.
/// Etapa 1: cada prueba crea su propia Tienda (sin fixture ni dobles de prueba).
public class TiendaTests
{
    [Fact]
    public void AgregarProducto_ProductoEstaEnElInventario()
    {
        var tienda = new Tienda();
        var producto = new Producto("Pan", 1000, "Almacén");

        tienda.AgregarProducto(producto);

        Assert.Contains(tienda.Inventario, p => p.Nombre == "Pan");
        var agregado = tienda.Inventario.Single();
        Assert.Equal("Pan", agregado.Nombre);
        Assert.Equal(100, agregado.Precio);
        Assert.Equal("Almacén", agregado.Categoria);
    }

    [Fact]
    public void AgregarProducto_AsignaIdsIncrementales()
    {
        var tienda = new Tienda();
        tienda.AgregarProducto(new Producto("Pan", 1000, "Almacén"));
        tienda.AgregarProducto(new Producto("Leche", 2000, "Lácteos"));

        Assert.Equal(1, tienda.Inventario[0].IdProducto);
        Assert.Equal(2, tienda.Inventario[1].IdProducto);
    }

    [Fact]
    public void BuscarProducto_ProductoExistente_RetornaElProducto()
    {
        var tienda = new Tienda();
        var producto = new Producto("Pan", 1000, "Almacén");
        tienda.AgregarProducto(producto);

        var resultado = tienda.BuscarProducto("Pan");

        Assert.NotNull(resultado);
        Assert.Same(producto, resultado);
    }

    [Fact]
    public void BuscarProducto_ProductoNoExistente_RetornaNull()
    {
        var tienda = new Tienda();
        tienda.AgregarProducto(new Producto("Pan", 1000, "Almacén"));

        var resultado = tienda.BuscarProducto("Azúcar");

        Assert.Null(resultado);
    }

    [Fact]
    public void EliminarProducto_ProductoAgregado_RetornaTrue()
    {
        var tienda = new Tienda();
        tienda.AgregarProducto(new Producto("Pan", 1000, "Almacén"));
        var id = tienda.Inventario[0].IdProducto;

        var eliminado = tienda.EliminarProducto(id);

        Assert.True(eliminado);
        Assert.Empty(tienda.Inventario);
    }

    [Fact]
    public void EliminarProducto_IdInexistente_RetornaFalse()
    {
        var tienda = new Tienda();

        var eliminado = tienda.EliminarProducto(99);

        Assert.False(eliminado);
    }
}
