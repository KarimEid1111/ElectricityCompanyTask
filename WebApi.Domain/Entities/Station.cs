namespace WebApi.Domain.Entities;

public partial class Station
{
    public int StationKey { get; set; }

    public int? CityKey { get; set; }

    public string? StationName { get; set; }

    public virtual City? CityKeyNavigation { get; set; }

    public virtual ICollection<Tower> Towers { get; set; } = new List<Tower>();
}
