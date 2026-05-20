namespace SistemaPaisa.Domain.Entities;

public class ProfileAction
{
    public int ProfileId { get; set; }
    public Profile Profile { get; set; } = null!;
    public int ActionId { get; set; }
    public ActionEntity Action { get; set; } = null!;
}
