

namespace ConsoleApp.Domain.Entities;

public class City
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int ZoneId { get; set; }

    public virtual ICollection<Station> Stations { get; set; } = new List<Station>();

    public virtual Zone Zone { get; set; }
}
