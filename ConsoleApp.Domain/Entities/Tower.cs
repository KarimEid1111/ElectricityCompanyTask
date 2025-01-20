namespace ConsoleApp.Domain.Entities;

public class Tower
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int StationId { get; set; }

    public virtual ICollection<Cabin> Cabins { get; set; } = new List<Cabin>();

    public virtual Station Station { get; set; }
}
