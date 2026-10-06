using Carrito;

namespace UnitTestingxUnit.Tests;

/// Pruebas de integración del carrito de compras (Etapa 5).
/// Cada clase de pruebas recibe su propia instancia del fixture,
/// por lo que los cambios de precio de estas pruebas no afectan a TiendaTests.
public class CarritoTests : IClassFixture<TiendaFixture>
{
    private readonly TiendaFixture _fixture;

    public CarritoTests(TiendaFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void CalcularTotalCarrito_ConDescuentoAplicado_SumaElPrecioActualizado()
    {
        // Flujo completo: tienda del fixture -> descuento -> carrito por nombre -> total.
        _fixture.Tienda.AplicarDescuento("Pan", 10m);

        var carrito = new List<string> { "Pan", "Leche" };

        var total = _fixture.Tienda.CalcularTotalCarrito(carrito);

        // Pan: 1000 - 10% = 900; Leche: 2000 sin cambios.
        Assert.Equal(2900m, total);
    }

    [Fact]
    public void CalcularTotalCarrito_ConPrecioActualizado_UsaElNuevoPrecio()
    {
        // El total también debe reflejar los cambios hechos con ActualizarPrecio (Etapa 2).
        var queso = _fixture.Tienda.BuscarProducto("Queso");
        queso.ActualizarPrecio(2500m);

        var carrito = new List<string> { "Queso", "Leche" };

        var total = _fixture.Tienda.CalcularTotalCarrito(carrito);

        // Queso: 2500 tras la actualización; Leche: 2000 sin cambios.
        Assert.Equal(4500m, total);
    }

    [Fact]
    public void CalcularTotalCarrito_ProductoNoExistente_LanzaExcepcion()
    {
        // El carrito conserva el comportamiento de excepciones de la Etapa 2.
        var carrito = new List<string> { "Azúcar" };

        var ex = Assert.Throws<ManejarExcepciones>(() => _fixture.Tienda.CalcularTotalCarrito(carrito));

        Assert.Contains("Azúcar", ex.Message);
    }

    [Fact]
    public void CalcularTotalCarrito_ConVariosDescuentos_SumaLosPreciosDescontados()
    {
        // Estado inicial del fixture (Etapa 5): Yerba 800 y Café 1200.
        // Dos descuentos: Yerba 10% -> 720 y Café 50% -> 600.
        _fixture.Tienda.AplicarDescuento("Yerba", 10m);
        _fixture.Tienda.AplicarDescuento("Café", 50m);

        var carrito = new List<string> { "Yerba", "Café", "Leche" };

        var total = _fixture.Tienda.CalcularTotalCarrito(carrito);

        // 720 + 600 + 2000 = 3320; Leche no tiene descuento.
        Assert.Equal(3320m, total);
    }
}
