using Carrito;
using Moq;

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
        Assert.Equal(1000, agregado.Precio);
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
    public void BuscarProducto_ProductoNoExistente_LanzaExcepcion()
    {
        // Implementación anterior - Etapa 1:
        // var resultado = tienda.BuscarProducto("Azúcar");
        // Assert.Null(resultado);

        // Etapa 2: ahora se espera una excepción cuando el producto no existe.
        var tienda = new Tienda();
        tienda.AgregarProducto(new Producto("Pan", 1000, "Almacén"));

        var ex = Assert.Throws<ManejarExcepciones>(() => tienda.BuscarProducto("Azúcar"));
        Assert.Contains("Azúcar", ex.Message);
    }

    [Fact]
    public void EliminarProducto_ProductoAgregado_NoPermaneceEnElInventario()
    {
        // Implementación anterior - Etapa 1:
        // var eliminado = tienda.EliminarProducto(id);
        // Assert.True(eliminado);

        // Etapa 2: EliminarProducto ya no devuelve bool; solo debe eliminar sin lanzar.
        var tienda = new Tienda();
        tienda.AgregarProducto(new Producto("Pan", 1000, "Almacén"));
        var id = tienda.Inventario[0].IdProducto;

        tienda.EliminarProducto(id);
        Assert.Empty(tienda.Inventario);
    }

    [Fact]
    public void EliminarProducto_IdInexistente_LanzaExcepcion()
    {
        // Implementación anterior - Etapa 1:
        // var eliminado = tienda.EliminarProducto(99);
        // Assert.False(eliminado);

        // Etapa 2: ahora se espera una excepción cuando el producto no existe.
        var tienda = new Tienda();

        Assert.Throws<ManejarExcepciones>(() => tienda.EliminarProducto(99));
    }

    [Fact]
    public void ActualizarPrecio_PrecioNegativo_LanzaExcepcion()
    {
        var producto = new Producto("Pan", 1000, "Almacén");

        Assert.Throws<ManejarExcepciones>(() => producto.ActualizarPrecio(-1m));

        // El precio no debe haber cambiado.
        Assert.Equal(1000, producto.Precio);
    }

    [Fact]
    public void ActualizarPrecio_PrecioValido_CambiaElPrecio()
    {
        var producto = new Producto("Pan", 1000, "Almacén");

        producto.ActualizarPrecio(1500);

        Assert.Equal(1500, producto.Precio);
    }

    [Fact]
    public void AplicarDescuento_LlamaActualizarPrecio_ConPrecioCalculado()
    {
        // Doble de prueba: no se usa un Producto real como sujeto bajo prueba.
        var productoMock = new Mock<Producto>("Pan", 100m, "Almacén");
        var tienda = new Tienda();
        tienda.AgregarProducto(productoMock.Object);

        tienda.AplicarDescuento("Pan", 20m);

        // Verifica la interacción: ActualizarPrecio se invoca una vez con 100 - 20% = 80.
        productoMock.Verify(p => p.ActualizarPrecio(80m), Times.Once);
    }

    [Fact]
    public void AplicarDescuento_PrecioReal100_Descuento20_NuevoPrecio80()
    {
        var tienda = new Tienda();
        var producto = new Producto("Pan", 100m, "Almacén");
        tienda.AgregarProducto(producto);

        tienda.AplicarDescuento("Pan", 20m);

        Assert.Equal(80m, producto.Precio);
    }

    [Fact]
    public void AplicarDescuento_ProductoNoExistente_LanzaExcepcion()
    {
        var tienda = new Tienda();

        Assert.Throws<ManejarExcepciones>(() => tienda.AplicarDescuento("Azúcar", 10m));
    }
}
