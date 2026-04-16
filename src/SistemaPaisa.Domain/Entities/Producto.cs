namespace SistemaPaisa.Domain.Entities;

public class Producto : BaseEntity
{
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public int Stock { get; set; }
    public bool Activo { get; set; } = true;
    public int? CategoriaId { get; set; }
    public Categoria? Categoria { get; set; }
}