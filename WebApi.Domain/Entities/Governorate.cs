namespace WebApi.Domain.Entities;

public partial class Governorate
{
    public int GovernrateKey { get; set; }

    public string? GovernrateName { get; set; }

    public virtual ICollection<Sector> Sectors { get; set; } = new List<Sector>();
}
