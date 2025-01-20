namespace ConsoleApp.Domain.Entities;

public class Station
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int CityId { get; set; }

    public virtual City City { get; set; }

    public virtual ICollection<Tower> Towers { get; set; } = new List<Tower>();
}
