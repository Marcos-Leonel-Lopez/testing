namespace Carrito;

/// Representa un producto de la tienda.
/// El ID lo asigna Tienda al agregarlo al inventario.
public class Producto
{
    /// Identificador único del producto dentro de la tienda.
    public int IdProducto { get; set; }

    /// Nombre del producto.
    public string Nombre { get; }

    // Implementación anterior - Etapa 1:
    // public decimal Precio { get; }

    // Etapa 2: el precio ahora es actualizable mediante ActualizarPrecio.
    public decimal Precio { get; private set; }

    // Implementación anterior - Etapa 2:
    // public void ActualizarPrecio(decimal nuevoPrecio)

    // Etapa 3: virtual para permitir sustitución del producto en pruebas (test double/mock).
    /// Actualiza el precio. Lanza ManejarExcepciones si el nuevo precio es negativo.
    public virtual void ActualizarPrecio(decimal nuevoPrecio)
    {
        if (nuevoPrecio < 0)
        {
            throw new ManejarExcepciones($"El precio no puede ser negativo: {nuevoPrecio}.");
        }

        Precio = nuevoPrecio;
    }

    /// Categoría del producto.
    public string Categoria { get; }

    public Producto(string nombre, decimal precio, string categoria)
    {
        Nombre = nombre;
        Precio = precio;
        Categoria = categoria;
    }
}
