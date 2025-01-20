namespace ConsoleApp.Domain.Entities;

public class Cable
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int CabinId { get; set; }

    public virtual ICollection<Block> Blocks { get; set; } = new List<Block>();

    public virtual Cabin Cabin { get; set; }
}
