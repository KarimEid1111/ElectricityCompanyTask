namespace ConsoleApp.Domain.Entities;

public class Zone
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int SectorId { get; set; }

    public virtual ICollection<City> Cities { get; set; } = new List<City>();

    public virtual Sector Sector { get; set; }
}
