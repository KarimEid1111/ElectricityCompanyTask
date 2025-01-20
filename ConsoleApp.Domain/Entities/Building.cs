
namespace ConsoleApp.Domain.Entities;

public class Building
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int BlockId { get; set; }

    public virtual Block Block { get; set; }

    public virtual ICollection<Flat> Flats { get; set; } = new List<Flat>();

    public virtual ICollection<Subscribtion> Subscribtions { get; set; } = new List<Subscribtion>();
}
