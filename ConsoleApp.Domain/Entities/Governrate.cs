

namespace ConsoleApp.Domain.Entities;

public class Governrate
{
    public int Id { get; set; }

    public string Name { get; set; }

    public virtual ICollection<Sector> Sectors { get; set; } = new List<Sector>();
}
