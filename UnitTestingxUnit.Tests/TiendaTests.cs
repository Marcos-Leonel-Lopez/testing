using Carrito;
using Moq;

namespace UnitTestingxUnit.Tests;

/// Pruebas de las operaciones básicas de Tienda y Producto.
/// Etapa 4: usa IClassFixture<TiendaFixture> para reutilizar el estado inicial;
/// las pruebas destructivas y las de descuento conservan su propia Tienda.
public class TiendaTests : IClassFixture<TiendaFixture>
{
    private readonly TiendaFixture _fixture;

    public TiendaTests(TiendaFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void AgregarProducto_ProductoEstaEnElInventario()
    {
        // Implementación anterior - Etapa 3:
        // var tienda = new Tienda();
        // var producto = new Producto("Pan", 1000, "Almacén");
        //
        // tienda.AgregarProducto(producto);
        //
        // Assert.Contains(tienda.Inventario, p => p.Nombre == "Pan");
        // var agregado = tienda.Inventario.Single();
        // Assert.Equal("Pan", agregado.Nombre);
        // Assert.Equal(1000, agregado.Precio);
        // Assert.Equal("Almacén", agregado.Categoria);

        // Etapa 4: se agrega a la tienda del fixture; "Arroz" es un nombre exclusivo de esta prueba
        // y Single() se filtra por nombre porque el inventario ya trae 3 productos.
        var producto = new Producto("Arroz", 1500, "Almacén");

        _fixture.Tienda.AgregarProducto(producto);

        Assert.Contains(_fixture.Tienda.Inventario, p => p.Nombre == "Arroz");
        var agregado = _fixture.Tienda.Inventario.Single(p => p.Nombre == "Arroz");
        Assert.Equal("Arroz", agregado.Nombre);
        Assert.Equal(1500, agregado.Precio);
        Assert.Equal("Almacén", agregado.Categoria);
    }

    [Fact]
    public void AgregarProducto_AsignaIdsIncrementales()
    {
        // Implementación anterior - Etapa 3:
        // var tienda = new Tienda();
        // tienda.AgregarProducto(new Producto("Pan", 1000, "Almacén"));
        // tienda.AgregarProducto(new Producto("Leche", 2000, "Lácteos"));
        //
        // Assert.Equal(1, tienda.Inventario[0].IdProducto);
        // Assert.Equal(2, tienda.Inventario[1].IdProducto);

        // Etapa 4: los IDs se verifican de forma relativa al máximo existente en la tienda del fixture,
        // porque otras pruebas pueden haber agregado productos antes.
        var maxId = _fixture.Tienda.Inventario.Max(p => p.IdProducto);

        _fixture.Tienda.AgregarProducto(new Producto("Aceite", 2500, "Almacén"));
        _fixture.Tienda.AgregarProducto(new Producto("Harina", 1200, "Almacén"));

        var aceite = _fixture.Tienda.Inventario.Single(p => p.Nombre == "Aceite");
        var harina = _fixture.Tienda.Inventario.Single(p => p.Nombre == "Harina");
        Assert.Equal(maxId + 1, aceite.IdProducto);
        Assert.Equal(maxId + 2, harina.IdProducto);
    }

    [Fact]
    public void BuscarProducto_ProductoExistente_RetornaElProducto()
    {
        // Implementación anterior - Etapa 3:
        // var tienda = new Tienda();
        // var producto = new Producto("Pan", 1000, "Almacén");
        // tienda.AgregarProducto(producto);
        //
        // var resultado = tienda.BuscarProducto("Pan");
        //
        // Assert.NotNull(resultado);
        // Assert.Same(producto, resultado);

        // Etapa 4: se busca en la tienda del fixture y se compara con la referencia predefinida.
        var resultado = _fixture.Tienda.BuscarProducto("Pan");

        Assert.NotNull(resultado);
        Assert.Same(_fixture.Pan, resultado);
    }

    [Fact]
    public void BuscarProducto_ProductoNoExistente_LanzaExcepcion()
    {
        // Implementación anterior - Etapa 3:
        // var tienda = new Tienda();
        // tienda.AgregarProducto(new Producto("Pan", 1000, "Almacén"));

        // Etapa 4: se busca en la tienda del fixture; "Azúcar" no está predefinido.

        // Implementación anterior - Etapa 1:
        // var resultado = tienda.BuscarProducto("Azúcar");
        // Assert.Null(resultado);

        // Etapa 2: ahora se espera una excepción cuando el producto no existe.
        var ex = Assert.Throws<ManejarExcepciones>(() => _fixture.Tienda.BuscarProducto("Azúcar"));
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
        var productoMock = new Mock<Producto>("Pan", 1000m, "Almacén");
        var tienda = new Tienda();
        tienda.AgregarProducto(productoMock.Object);

        tienda.AplicarDescuento("Pan", 20);

        // Verifica la interacción: ActualizarPrecio se invoca una vez con 100 - 20% = 80.
        productoMock.Verify(p => p.ActualizarPrecio(800m), Times.Once);
    }

    [Fact]
    public void AplicarDescuento_PrecioReal100_Descuento20_NuevoPrecio80()
    {
        var tienda = new Tienda();
        var producto = new Producto("Pan", 1000, "Almacén");
        tienda.AgregarProducto(producto);

        tienda.AplicarDescuento("Pan", 20);

        Assert.Equal(800, producto.Precio);
    }

    [Fact]
    public void AplicarDescuento_ProductoNoExistente_LanzaExcepcion()
    {
        var tienda = new Tienda();

        Assert.Throws<ManejarExcepciones>(() => tienda.AplicarDescuento("Azúcar", 1000));
    }
}
