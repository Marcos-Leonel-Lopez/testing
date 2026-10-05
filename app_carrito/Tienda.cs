namespace Carrito;

/// Gestiona el inventario de productos de la tienda (en memoria).
public class Tienda
{
    private readonly List<Producto> _inventario = new();
    private int _siguienteId = 1;

    /// Vista de solo lectura del inventario actual.
    public IReadOnlyList<Producto> Inventario => _inventario;

    /// Agrega un producto al inventario y le asigna el siguiente ID.
    public void AgregarProducto(Producto producto)
    {
        producto.IdProducto = _siguienteId++;
        _inventario.Add(producto);
    }

    /// Busca un producto por nombre exacto. Lanza ManejarExcepciones si no existe.
    public Producto BuscarProducto(string nombre)
    {
        // Implementación anterior - Etapa 1:
        // return _inventario.FirstOrDefault(p => p.Nombre == nombre);

        // Etapa 2: ahora se lanza una excepción cuando el producto no existe.
        var producto = _inventario.FirstOrDefault(p => p.Nombre == nombre);
        if (producto is null)
        {
            throw new ManejarExcepciones($"No existe un producto llamado '{nombre}'.");
        }

        return producto;
    }

    /// Elimina el producto con el ID indicado. Lanza ManejarExcepciones si no existe.
    public void EliminarProducto(int idProducto)
    {
        // Implementación anterior - Etapa 1:
        // var producto = _inventario.FirstOrDefault(p => p.IdProducto == idProducto);
        // if (producto is null)
        // {
        //     return false;
        // }
        // _inventario.Remove(producto);
        // return true;

        // Etapa 2: ahora se lanza una excepción cuando el producto no existe.
        var producto = _inventario.FirstOrDefault(p => p.IdProducto == idProducto);
        if (producto is null)
        {
            throw new ManejarExcepciones($"No existe un producto con ID {idProducto}.");
        }

        _inventario.Remove(producto);
    }

    /// Aplica un descuento porcentual al precio del producto. Lanza ManejarExcepciones si el producto no existe.
    public void AplicarDescuento(string nombreProducto, decimal porcentajeDescuento)
    {
        var producto = BuscarProducto(nombreProducto);
        var nuevoPrecio = producto.Precio - (producto.Precio * porcentajeDescuento / 100);

        // Etapa 3: se utiliza ActualizarPrecio para centralizar la modificación del precio.
        producto.ActualizarPrecio(nuevoPrecio);
    }
}
