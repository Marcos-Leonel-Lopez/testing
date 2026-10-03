namespace Carrito;

/// Excepción del dominio de la tienda para operaciones inválidas.
public class ManejarExcepciones : Exception
{
    public ManejarExcepciones(string mensaje) : base(mensaje)
    {
    }
}
