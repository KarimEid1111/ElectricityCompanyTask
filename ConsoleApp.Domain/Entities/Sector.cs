namespace ConsoleApp.Domain.Entities;

public class Sector
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int GovernrateId { get; set; }

    public virtual Governrate Governrate { get; set; }

    public virtual ICollection<Zone> Zones { get; set; } = new List<Zone>();
}
