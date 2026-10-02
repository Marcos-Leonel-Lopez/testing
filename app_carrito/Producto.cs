namespace Carrito;

/// Representa un producto de la tienda.
/// El ID lo asigna Tienda al agregarlo al inventario.
public class Producto
{
    /// Identificador único del producto dentro de la tienda.
    public int IdProducto { get; set; }

    /// Nombre del producto.
    public string Nombre { get; }

    public decimal Precio { get; }

    /// Categoría del producto.
    public string Categoria { get; }

    public Producto(string nombre, decimal precio, string categoria)
    {
        Nombre = nombre;
        Precio = precio;
        Categoria = categoria;
    }
}
