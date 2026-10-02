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

    /// Busca un producto por nombre exacto. Devuelve null si no existe.
    public Producto? BuscarProducto(string nombre)
    {
        return _inventario.FirstOrDefault(p => p.Nombre == nombre);
    }

    /// Elimina el producto con el ID indicado. Devuelve false si no existe.
    public bool EliminarProducto(int idProducto)
    {
        var producto = _inventario.FirstOrDefault(p => p.IdProducto == idProducto);
        if (producto is null)
        {
            return false;
        }
        _inventario.Remove(producto);
        return true;
    }
}
