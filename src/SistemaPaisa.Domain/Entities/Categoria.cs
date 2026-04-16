namespace SistemaPaisa.Domain.Entities;

public class Categoria : BaseEntity
{
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public bool Activa { get; set; } = true;
}
